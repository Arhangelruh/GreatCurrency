using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
    /// <inheritdoc cref="ICurrencyService"/>
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRequestService _requestService;

        public CurrencyService(IRepository<Currency> currencyRepository, IRequestService requestService)
        {
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        public async Task AddCurrencyAsync(CurrencyDto currencyDto)
        {
            if (currencyDto is null)
            {
                throw new ArgumentNullException(nameof(currencyDto));
            }

            var newCurrency = new Currency
            {
                BankDepartmentId = currencyDto.BankDepartmentId,
                USDBuyRate = currencyDto.USDBuyRate,
                USDSaleRate = currencyDto.USDSaleRate,
                EURBuyRate = currencyDto.EURBuyRate,
                EURSaleRate = currencyDto.EURSaleRate,
                RUBBuyRate = currencyDto.RUBBuyRate,
                RUBSaleRate = currencyDto.RUBSaleRate,
                RequestId = currencyDto.RequestId
            };

            await _currencyRepository.AddAsync(newCurrency);
            await _currencyRepository.SaveChangesAsync();
        }

        public async Task DeleteCurrenciesAsync(DateTime date)
        {
            var getRequests = await _requestService.GetRequestByDateAsync(date);

            if (getRequests.Any())
            {                
                foreach (var request in getRequests)
                {
                    var getCurrencies = await _currencyRepository
                   .GetAll()
                   .Where(currency => currency.RequestId == request.Id)
                   .AsNoTracking()
                   .ToListAsync();

                    if (getCurrencies.Any())
                    {
                        foreach (var currency in getCurrencies)
                        {
                            _currencyRepository.Delete(currency);
                            await _currencyRepository.SaveChangesAsync();                           
                        }
                    }
                    await _requestService.DeleteRequestAsync(request.Id);
                }
            }
        }

        public async Task<List<CurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end)
        {            
            var getRequests = await _requestService.GetRequestByDateBetweenAsync(begin,end);
            List<CurrencyDto> currencies = [];

            if (getRequests.Any()) {                

                foreach (var request in getRequests)
                {
                    var getCurrencies = await _currencyRepository
                       .GetAll()
                       .Where(currency => currency.RequestId == request.Id)
                       .AsNoTracking()
                       .ToListAsync();

                    if (getCurrencies.Any()) {
                        
                        foreach (var currency in getCurrencies)
                        {
                            currencies.Add(new CurrencyDto
                            {
                                Id = currency.Id,
                                BankDepartmentId = currency.BankDepartmentId,
                                USDBuyRate = currency.USDBuyRate,
                                USDSaleRate = currency.USDSaleRate,
                                EURBuyRate = currency.EURBuyRate,
                                EURSaleRate = currency.EURSaleRate,
                                RUBBuyRate = currency.RUBBuyRate,
                                RUBSaleRate = currency.RUBSaleRate,
                                RequestId = currency.RequestId
                            });
                        }
                    }
                }                
            }
            return currencies;
        }
    }
}
