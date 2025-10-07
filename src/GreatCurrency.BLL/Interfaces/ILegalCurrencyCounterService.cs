using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ILegalCurrencyCounterService
	{
		/// <summary>
		/// Get best rates and statistic.
		/// </summary>
		/// <param name="startDate">Start date</param>
		/// <param name="endDate">End date</param>
		/// <param name="serviceid">Id service</param>
		/// <returns>Rate statistic dto model</returns>
		Task<LegalRatesStatisticDto?> LegalCurrencyCounterAsync(DateTime startDate, DateTime endDate, int id);
	}
}
