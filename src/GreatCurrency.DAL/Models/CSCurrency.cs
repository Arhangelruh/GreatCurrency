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
