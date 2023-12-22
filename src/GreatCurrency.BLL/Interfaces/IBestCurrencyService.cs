using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface IBestCurrencyService
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
        Task<List<BestCurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end, int cityId);

        /// <summary>
        /// Add currency.
        /// </summary>
        /// <param name="currencyDto">Dto model</param>
        Task AddCurrencyAsync(BestCurrencyDto currencyDto);
    }
}
