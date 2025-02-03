using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface IChartService
	{
		/// <summary>
		/// Get all rates.
		/// </summary>
		/// <returns>list rates</returns>
		Task<List<LineChartDto>> GetAllRatesAsync(DateTime begin, DateTime end, int cityid, int bankId);
	}
}
