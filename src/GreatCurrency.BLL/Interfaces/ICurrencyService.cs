using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Delete currencies before time in request.
        /// </summary>
        /// <param name="date">Date</param>
        Task DeleteCurrenciesAsync(DateTime date);

        /// <summary>
        /// Get all currency.
        /// </summary>
        /// <returns>list Currencies</returns>
        Task<List<CurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end);

        /// <summary>
        /// Add currency.
        /// </summary>
        /// <param name="currencyDto">Dto model</param>
        Task AddCurrencyAsync(CurrencyDto currencyDto);
    }
}
