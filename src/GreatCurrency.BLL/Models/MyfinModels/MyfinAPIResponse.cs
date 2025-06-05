namespace GreatCurrency.BLL.Models.MyfinModels
{
	public class MyfinAPIResponse
	{
		/// <summary>
		/// List currency rates.
		/// </summary>
		public List<MyfinAPICurrencyRates> Data { get; set; }

		/// <summary>
		/// Total pages.
		/// </summary>
		public int TotalPages { get; set; }

		/// <summary>
		/// Current page.
		/// </summary>
		public int CurrentPage { get; set; }
	}
}
