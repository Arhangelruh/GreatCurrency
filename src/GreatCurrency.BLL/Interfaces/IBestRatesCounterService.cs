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
    }
}
