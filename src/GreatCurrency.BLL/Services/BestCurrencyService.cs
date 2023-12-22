using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
    public class BestCurrencyService : IBestCurrencyService
    {
        private readonly IRepository<BestCurrency> _bestCurrencyRepository;
        private readonly IRequestService _requestService;

        public BestCurrencyService(IRepository<BestCurrency> bestCurrencyRepository, IRequestService requestService)
        {
            _bestCurrencyRepository = bestCurrencyRepository ?? throw new ArgumentNullException(nameof(bestCurrencyRepository));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        public async Task AddCurrencyAsync(BestCurrencyDto bestCurrencyDto)
        {
            if (bestCurrencyDto is null)
            {
                throw new ArgumentNullException(nameof(bestCurrencyDto));
            }

            var newCurrency = new BestCurrency
            {
                BankId = bestCurrencyDto.BankId,
                USDBuyRate = bestCurrencyDto.USDBuyRate,
                USDSaleRate = bestCurrencyDto.USDSaleRate,
                EURBuyRate = bestCurrencyDto.EURBuyRate,
                EURSaleRate = bestCurrencyDto.EURSaleRate,
                RUBBuyRate = bestCurrencyDto.RUBBuyRate,
                RUBSaleRate = bestCurrencyDto.RUBSaleRate,
                RequestId = bestCurrencyDto.RequestId,
                CityId = bestCurrencyDto.CityId
            };

            await _bestCurrencyRepository.AddAsync(newCurrency);
            await _bestCurrencyRepository.SaveChangesAsync();
        }

        public async Task DeleteCurrenciesAsync(DateTime date)
        {
            var getRequests = await _requestService.GetRequestByDateAsync(date);
            if (getRequests.Any())
            {
                foreach (var request in getRequests)
                {
                    var getCurrencies = await _bestCurrencyRepository
                       .GetAll()
                       .Where(currency => currency.RequestId == request.Id)
                       .AsNoTracking()
                       .ToListAsync();

                    if (getCurrencies.Any())
                    {
                        foreach (var currency in getCurrencies)
                        {
                            _bestCurrencyRepository.Delete(currency);
                            await _bestCurrencyRepository.SaveChangesAsync();                            
                        }
                    }
                    await _requestService.DeleteRequestAsync(request.Id);
                }
            }
        }

        public async Task<List<BestCurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end, int cityId)
        {
            var getRequests = await _requestService.GetRequestByDateBetweenAsync(begin, end);
            List<BestCurrencyDto> currencies = [];

            if (getRequests.Any())
            {

                foreach (var request in getRequests.OrderBy(r=>r.IncomingDate).ToList())
                {
                    var getCurrencies = await _bestCurrencyRepository
                       .GetAll()
                       .Where(currency => currency.RequestId == request.Id && currency.CityId == cityId)
                       .AsNoTracking()
                       .ToListAsync();

                    if (getCurrencies.Any())
                    {

                        foreach (var currency in getCurrencies)
                        {
                            currencies.Add(new BestCurrencyDto
                            {
                                Id = currency.Id,
                                BankId = currency.BankId,
                                USDBuyRate = currency.USDBuyRate,
                                USDSaleRate = currency.USDSaleRate,
                                EURBuyRate = currency.EURBuyRate,
                                EURSaleRate = currency.EURSaleRate,
                                RUBBuyRate = currency.RUBBuyRate,
                                RUBSaleRate = currency.RUBSaleRate,
                                RequestId = currency.RequestId,
                                RequestTime = request.IncomingDate,
                                CityId = currency.CityId
                            });
                        }
                    }
                }
            }
            return currencies;
        }
    }
}

