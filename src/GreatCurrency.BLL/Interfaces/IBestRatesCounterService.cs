using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface IBestRatesCounterService
    {
        /// <summary>
        /// Get best rates and statistic.
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="bankId">Id bank</param>
        /// <returns>Rate statistic dto model</returns>
        Task<RateStatisticDto> BestCurrencyCounterAsync(DateTime startDate, DateTime endDate, int bankId, int cityId);

        /// <summary>
        /// Chek is there bank in list with best rates.
        /// </summary>
        /// <param name="bestCurrencies">Best rates</param>
        /// <param name="bankId">Bank Id</param>
        /// <returns>Best currency dto model need bank or random bank.</returns>
        BestCurrencyDto sortBestRates(List<BestCurrencyDto> bestCurrencies, int bankId);

        /// <summary>
        /// Get times when need bank used best courses.
        /// </summary>
        /// <param name="list">List of Best rates</param>
        /// <returns></returns>
        List<TimeRates> GetTimeRates(List<BestCurrencyDto> list, int bankId);        
    }
}
