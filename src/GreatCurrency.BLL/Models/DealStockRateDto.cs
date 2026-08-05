namespace GreatCurrency.BLL.Models
{
	public class DealStockRateDto
	{
		/// <summary>
		/// Currency.
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Rate.
		/// </summary>
		public decimal Rate { get; set; }

		/// <summary>
		/// Request id.
		/// </summary>
		public int RequestId { get; set; }

		/// <summary>
		/// Date time.
		/// </summary>
		public DateTime RequestTime { get; set; }
	}
}
