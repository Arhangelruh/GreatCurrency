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

        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
        }

        public async Task AddCurrencyAsync(CurrencyDto currencyDto)
        {
            if (currencyDto is null)
            {
                throw new ArgumentNullException(nameof(currencyDto));
            }

            var dateRequest = DateTime.Now;

            var newCurrency = new Currency
            {
                BankDepartmentId = currencyDto.BankDepartmentId,
                IncomingDate = dateRequest,
                USDBuyRate = currencyDto.USDBuyRate,
                USDSaleRate = currencyDto.USDSaleRate,
                EURBuyRate = currencyDto.EURBuyRate,
                EURSaleRate = currencyDto.EURSaleRate,
                RUBBuyRate = currencyDto.RUBBuyRate,
                RUBSaleRate = currencyDto.RUBSaleRate,
            };

            await _currencyRepository.AddAsync(newCurrency);
            await _currencyRepository.SaveChangesAsync();
        }

        public async Task DeleteCurrenciesAsync(DateTime date)
        {
            var getCurrencies = await _currencyRepository
               .GetAll()
               .AsNoTracking()
               .Where(currency => currency.IncomingDate <= date)
               .ToListAsync();

            if (getCurrencies.Any())
            {
                foreach (var currency in getCurrencies)
                {
                    _currencyRepository.Delete(currency);
                    await _currencyRepository.SaveChangesAsync();
                }
            }
        }

        public async Task<List<CurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end)
        {
            List<CurrencyDto> currencies = [];

            var getCurrencies = await _currencyRepository
                .GetAll()
                .AsNoTracking()
                .Where(currency => currency.IncomingDate >= begin && currency.IncomingDate <= end)
                .ToListAsync();

            if (!getCurrencies.Any())
            {
                return null;
            }
            foreach (var currency in getCurrencies)
            {
                currencies.Add(new CurrencyDto
                {
                    Id = currency.Id,
                    IncomingDate = currency.IncomingDate,
                    BankDepartmentId = currency.BankDepartmentId,
                    USDBuyRate = currency.USDBuyRate,
                    USDSaleRate = currency.USDSaleRate,
                    EURBuyRate = currency.EURBuyRate,
                    EURSaleRate = currency.EURSaleRate,
                    RUBBuyRate = currency.RUBBuyRate,
                    RUBSaleRate = currency.RUBSaleRate
                });
            }
            return currencies;
        }
    }
}
