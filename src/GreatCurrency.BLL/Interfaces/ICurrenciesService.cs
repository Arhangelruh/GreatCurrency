using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface ICurrenciesService
	{
		/// <summary>
		/// Add currency.
		/// </summary>
		/// <param name="currency">Currency model</param>
		/// <returns></returns>
		Task<int> AddCurrencyAsync(CurrenciesDto currency);

		/// <summary>
		/// Get all currencies.
		/// </summary>
		/// <returns>List of currencies</returns>
		Task<List<CurrenciesDto>> GetAllCurreciesAsync();

		/// <summary>
		/// Get currency by name.
		/// </summary>
		/// <returns></returns>
		Task<CurrenciesDto?> GetCurrencyByName(string name);

	}
}
