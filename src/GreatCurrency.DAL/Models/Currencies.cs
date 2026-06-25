using GreatCurrency.BLL.Models;

namespace GreatCurrency.DAL.Models
{
	public class Currencies
	{
		/// <summary>
		/// Currency Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Currency name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Navigation to deal stock rates.
		/// </summary>
		public ICollection<DealStockRates> dealStockRates { get; set; }
	}
}
