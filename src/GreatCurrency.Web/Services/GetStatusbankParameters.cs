namespace GreatCurrency.Web.Services
{
	/// <summary>
	/// Get credentials to myfin api.
	/// </summary>
	/// <param name="login">login parameter</param>
	/// <param name="password">password parameter</param>
	public class GetStatusbankParameters(string? login, string? password)
	{		
		public string Login = login ?? throw new ArgumentNullException(nameof(login));

		public string Password = password ?? throw new ArgumentNullException(nameof(password));		
	}
}
