namespace GreatCurrency.BLL.Models.MyfinModels
{
	public class MyfinAPIRate
	{
		/// <summary>
		/// Currency.
		/// </summary>
		public MyfinAPICurrency currency { get; set; }

		/// <summary>
		/// Currency buy.
		/// </summary>
		public double rate_buy { get; set; }

		/// <summary>
		/// Currency sell.
		/// </summary>
		public double rate_sell { get; set; }

		/// <summary>
		/// Update time.
		/// </summary>
		public int time_update { get; set; }

	}
}
