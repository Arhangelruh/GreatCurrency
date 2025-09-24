using GreatCurrency.BLL.Models.StatusBankAPIModels;

namespace GreatCurrency.BLL.Interfaces
{
	public interface IStatusBankAPIService
	{
		/// <summary>
		/// Get session token.
		/// </summary>
		/// <param name="login">Login</param>
		/// <param name="password">Password</param>
		/// <returns>Session token</returns>
		Task<StatusBankAPILogin> GetSessionTokenAsync(string login, string password);

		/// <summary>
		/// Get rates.
		/// </summary>
		/// <param name="token">Session token</param>
		/// <returns>StatusBank legal rates</returns>
		Task<QuotesResponse> GetRatesAsync(string token);
	}
}
