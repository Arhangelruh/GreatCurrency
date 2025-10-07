namespace GreatCurrency.BLL.Constants
{
	public class StatusBankAPILinksConstant
	{
		/// <summary>
		/// Get session token.
		/// </summary>
		public const string login = "https://fx.stbank.by/services/session/extLogin";

		/// <summary>
		/// Get all rates.
		/// </summary>
		public const string getRates = "https://fx.stbank.by/services/trading/getExtQuotesList";
	}
}
