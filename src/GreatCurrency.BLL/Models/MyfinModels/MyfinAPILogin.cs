namespace GreatCurrency.BLL.Models.MyfinModels
{
	/// <summary>
	/// Myfin api auth data.
	/// </summary>
	public class MyfinAPILogin
	{
		/// <summary>
		/// User.
		/// </summary>
		public MyfinAPIUser user { get; set; }

		/// <summary>
		/// Bearer token.
		/// </summary>
		public string token { get; set; }

		/// <summary>
		/// Bearer refresh token.
		/// </summary>
		public string refreshtoken { get; set; }
	}
}
