namespace GreatCurrency.DAL.Models
{
	public class CSCurrency
	{
		/// <summary>
		/// Currency Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Usd buy currency.
		/// </summary>
		public double USDBuyRate { get; set; }

		/// <summary>
		/// Usd sale currency.
		/// </summary>
		public double USDSaleRate { get; set; }

		/// <summary>
		/// Eur buy currency.
		/// </summary>
		public double EURBuyRate { get; set; }

		/// <summary>
		/// Eur sale currency.
		/// </summary>
		public double EURSaleRate { get; set; }

		/// <summary>
		/// RUB buy currency.
		/// </summary>
		public double RUBBuyRate { get; set; }

		/// <summary>
		/// RUB sale currency.
		/// </summary>
		public double RUBSaleRate { get; set; }

		/// <summary>
		/// EURUSD buy currency.
		/// </summary>
		public double EURUSDBuyRate { get; set; }

		/// <summary>
		/// EURUSD sell currency.
		/// </summary>
		public double EURUSDSellRate { get; set; }

		/// <summary>
		/// USDRUB buy currency.
		/// </summary>
		public double USDRUBBuyRate { get; set; }

		/// <summary>
		/// USDRUB sell currency.
		/// </summary>
		public double USDRUBSellRate { get; set; }

		/// <summary>
		/// EURRUB buy currency.
		/// </summary>
		public double EURRUBBuyRate { get; set; }

		/// <summary>
		/// EURRUB sell currency.
		/// </summary>
		public double EURRUBSellRate { get; set; }

		/// <summary>
		/// Navigate to currency service.
		/// </summary>
		public int CurrencyServicesId { get; set; }

		/// <summary>
		/// Navigate to currency service.
		/// </summary>
		public CurrencyService CurrencyService { get; set; }

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public int RequestId { get; set; }

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public SCRequest SCRequest { get; set; }
	}
}
