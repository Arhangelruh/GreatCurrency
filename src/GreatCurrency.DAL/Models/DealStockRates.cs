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
		/// Rate value.
		/// </summary>
		public decimal Rate { get; set; }

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public int RequestId { get; set; }

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public LERequest LERequest { get; set; }

		/// <summary>
		/// Navigate to currency.
		/// </summary>
		public int CurrencyId { get; set; }

		/// <summary>
		/// Navigate to currency.
		/// </summary>
		public Currencies Currency { get; set; }
	}
}
