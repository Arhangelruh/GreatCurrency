using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	/// <summary>
	/// Check best currencies, compare with us, send message.
	/// </summary>
	public interface ICheckCurrency
	{
		/// <summary>
		/// Main method for checking rates.
		/// </summary>
		/// <param name="mainBankId">our bank</param>
		/// <returns></returns>
		Task CheckCurrencyAsync(int mainBankId);

		/// <summary>
		/// Compare rates in city during banks.
		/// </summary>
		/// <param name="lastRequest">Last request currencies in city.</param>
		/// <param name="penultimateRequest">Penultimate request currencies in city.</param>
		/// <param name="mainBankId">Our bank.</param>
		/// <param name="cityName">City for compare currencies.</param>
		/// <returns></returns>
		Task<List<string>> CompareCurrenciesAsync(int lastRequest, int penultimateRequest, int mainBankId, string cityName);

		/// <summary>
		/// Get better rates than us.
		/// </summary>
		/// <param name="lastRequest">last request id</param>
		/// <param name="mainBankId">our bank id</param>
		/// <returns></returns>
		Task<List<RateDto>> GetBetterCoursesAsync(int lastRequest, int mainBankId);

		/// <summary>
		/// Get our rates from request.
		/// </summary>
		/// <param name="lastRequest">last requesi id</param>
		/// <param name="mainBankId">our bank id</param>
		/// <returns></returns>
		Task<BestCurrencyDto?> GetOurCourseAsync(int lastRequest, int mainBankId);

		/// <summary>
		/// Create better rates message.
		/// </summary>
		/// <param name="betterRates">list better rates.</param>
		/// <returns></returns>
		Task<string> CreateBetterRatesMessageAsync(List<RateDto> betterRates);

		/// <summary>
		/// Send message method.
		/// </summary>
		/// <param name="Message">message</param>
		/// <returns></returns>
		Task SendMessageAsync(string Message);
	}
}
