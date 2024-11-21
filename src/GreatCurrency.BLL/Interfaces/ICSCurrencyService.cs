using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ICSCurrencyService
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
		Task<List<CSCurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end);

		/// <summary>
		/// Add currency.
		/// </summary>
		/// <param name="cSCurrencyDto">Dto model</param>
		Task AddCurrencyAsync(CSCurrencyDto cSCurrencyDto);

		/// <summary>
		/// Get currencies with params.
		/// </summary>
		/// <param name="start">DataTime start date</param>
		/// <param name="end">DataTime end date</param>
		/// <param name="pageIndex">Page number</param>
		/// <param name="pageSize">Page size</param>
		/// <returns></returns>
		Task<List<CSCurrencyDto>> GetCurrenciesWithParamsAsync(DateTime start, DateTime end, int pageIndex, int pageSize);

		/// <summary>
		/// Get counts.
		/// </summary>
		/// <param name="begin">DateTime start</param>
		/// <param name="end">DateTime end</param>
		/// <returns></returns>
		Task<int> CurrencyServiceCountsAsync(DateTime begin, DateTime end);
	}
}
