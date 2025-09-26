using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ILECurrencyService
	{
		/// <summary>
		/// Delete currencies before time in request.
		/// </summary>
		/// <param name="date">Date</param>
		Task DeleteCurrenciesAsync(DateTime date);

		/// <summary>
		/// Get all currency.
		/// </summary>
		/// <param name="begin">Date from</param>
		/// <param name="end">Date to</param>
		/// <returns>list Currencies</returns>
		Task<List<LECurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end);

		/// <summary>
		/// Add currency.
		/// </summary>
		/// <param name="currencyDto">Dto model</param>
		Task AddCurrencyAsync(LECurrencyDto currencyDto);

		/// <summary>
		/// Get rates with pagination.
		/// </summary>
		/// <param name="begin">Date from</param>
		/// <param name="end">Date to</param>
		/// <param name="pageIndex">Index infornation</param>
		/// <param name="pageSize">Sixe information</param>
		/// <returns>List rates</returns>
		Task<List<LECurrencyDto>> GetCurrenciesAsync(DateTime begin, DateTime end, int pageIndex, int pageSize);

		/// <summary>
		/// Get table size between two dates.
		/// </summary>
		/// <param name="begin">Date from</param>
		/// <param name="end">Date to</param>
		/// <returns></returns>
		Task<int> CurrencyCountAsync(DateTime begin, DateTime end);
	}
}
