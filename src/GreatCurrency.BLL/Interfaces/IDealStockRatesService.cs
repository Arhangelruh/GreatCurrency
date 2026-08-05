using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface IDealStockRatesService
	{
		/// <summary>
		/// Add deal stock rates.
		/// </summary>
		/// <param name="dealRates">DealStockRates model</param>
		/// <returns></returns>
		Task AddDealStokRatesAsync(List<DealStockRateDto> dealRates);

		/// <summary>
		/// Delete deal stock rates.
		/// </summary>
		/// <param name="date">Date for deleting</param>
		/// <returns></returns>
		Task DeleteDealStockRatesAsync(DateTime date);

		/// <summary>
		/// Get deal stock rates bi time.
		/// </summary>
		/// <param name="begin">Start point of time period.</param>
		/// <param name="end">End point of time period.</param>
		/// <returns></returns>
		Task<List<DealStockRateDto>> GetRatesByTimeAsync(DateTime begin, DateTime end);
	}
}
