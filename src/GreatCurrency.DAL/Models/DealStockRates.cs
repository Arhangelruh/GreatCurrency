using GreatCurrency.DAL.Models;

namespace GreatCurrency.BLL.Models
{
	public class DealStockRates
	{
		/// <summary>
		/// Currency Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// USD Rate value.
		/// </summary>
		public decimal USDRate { get; set; }

		/// <summary>
		/// EUR Rate value.
		/// </summary>
		public decimal EURRate { get; set; }

		/// <summary>
		/// RUB Rate value.
		/// </summary>
		public decimal RUBRate { get; set; }

		/// <summary>
		/// CNY Rate value.
		/// </summary>
		public decimal CNYRate { get; set; }

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public int RequestId { get; set; }

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public LERequest LERequest { get; set; }
	}
}
