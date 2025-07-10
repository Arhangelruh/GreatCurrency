using GreatCurrency.BLL.Models.MyfinModels;

namespace GreatCurrency.BLL.Interfaces
{
	public interface IMyfinAPIService
	{
		/// <summary>
		/// Get bearer token.
		/// </summary>
		/// <param name="login">Login</param>
		/// <param name="password">Password</param>
		/// <returns>Bearer token</returns>
		Task<MyfinAPILogin> GetBearerToken(string login, string password);

		/// <summary>
		/// Get all banks.
		/// </summary>
		/// <param name="bearerToken">Bearer token</param>
		/// <returns>List of banks</returns>
		Task<List<MyfinAPIBank>> GetAllBanks(string bearerToken);

		/// <summary>
		/// Get all cities.
		/// </summary>
		/// <param name="bearerToken">Bearer token</param>
		/// <returns>List of cities</returns>
		Task<List<MyfinAPICity>> GetAllCities(string bearerToken);

		/// <summary>
		/// Get all rates by city.
		/// </summary>
		/// <param name="bearerToken">Bearer token</param>
		/// <param name="cityid">City Id</param>
		/// <returns>List of currency rates</returns>
		Task<List<MyfinAPICurrencyRates>> GetRateCurrenciesByCity(string bearerToken, int cityid);
	}
}
