using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    /// <summary>
    /// Class for working with best currencies data.
    /// </summary>
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

        /// <summary>
        /// Get last unic two requests by city.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<List<int>> GetLastTwoRequestsByCityAsync(int cityId);

        /// <summary>
        /// Get currencies by request id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>List of currencies</returns>
        Task<List<BestCurrencyDto>> GetCurrenciesByRequestAsync(int requestId);

        /// <summary>
        /// Get best currencies with params.
        /// </summary>
        /// <param name="begin">date from</param>
        /// <param name="end">date to</param>
        /// <param name="cityId">city id</param>
        /// <param name="pageIndex">current page</param>
        /// <param name="pageSize">page size</param>
        /// <returns></returns>
        Task<List<BestCurrencyDto>> GetBestCurrenciesAsync(DateTime begin, DateTime end, int cityId, int pageIndex, int pageSize);

        /// <summary>
        /// Get best currency counts.
        /// </summary>
        /// <returns></returns>
        Task<int> BestCurrencyCountsAsync(DateTime begin, DateTime end, int cityId);
	}
}
