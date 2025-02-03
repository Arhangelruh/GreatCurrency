namespace GreatCurrency.BLL.Models
{
	public class LineRateDto
	{
		/// <summary>
		/// Sale currency.
		/// </summary>
		public double OurSaleRate { get; set; }

		/// <summary>
		/// Best sale currency.
		/// </summary>
		public double BestSaleRate { get; set; }

		/// <summary>
		/// Buy currency.
		/// </summary>
		public double OurBuyRate { get; set; }

		/// <summary>
		/// Best buy currency.
		/// </summary>
		public double BestBuyRate { get; set; }

		/// <summary>
		/// Currency time.
		/// </summary>
		public string Time { get; set; }
	}
}
