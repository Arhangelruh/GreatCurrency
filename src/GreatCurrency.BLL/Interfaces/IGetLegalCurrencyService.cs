namespace GreatCurrency.BLL.Interfaces
{
	public interface IGetLegalCurrencyService
	{
		/// <summary>
		/// Get currencies and save in DB.
		/// </summary>
		/// <param name="login">Login for API</param>
		/// <param name="password">Password for API</param>
		/// <returns></returns>
		Task GetAndSaveAsync(string login, string password);
	}
}
