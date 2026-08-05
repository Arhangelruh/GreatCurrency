namespace GreatCurrency.Web.Models
{
	public class LineRate
	{
		/// <summary>
		/// Our sell rate.
		/// </summary>
		public double OurSellRate { get; set; }

		/// <summary>
		/// Our buy rate.
		/// </summary>
		public double OurBuyRate { get; set; }

		/// <summary>
		/// Stock deal rate.
		/// </summary>
		public decimal DealRate { get; set; }

		/// <summary>
		/// Currency time.
		/// </summary>
		public string Time { get; set; }
	}
}
