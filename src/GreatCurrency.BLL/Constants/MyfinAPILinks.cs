namespace GreatCurrency.BLL.Constants
{
	public class MyfinAPILinks
	{
		/// <summary>
		/// Get bearer token.
		/// </summary>
		public const string login = "https://partner-api.myfin.by/v1/auth/login";

		/// <summary>
		/// Get all banks.
		/// </summary>
		public const string getBanks = "https://partner-api.myfin.by/v1/bank";

		/// <summary>
		/// Get all cities.
		/// </summary>
		public const string getCities = "https://partner-api.myfin.by/v1/city";

		/// <summary>
		/// Get all rates.
		/// </summary>
		public const string getRates = "https://partner-api.myfin.by/v1/currency/rates";
	}
}
