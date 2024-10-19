namespace GreatCurrency.DAL.Models
{
	public class CurrencyServices
	{
		/// <summary>
		/// Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Bank Name.
		/// </summary>
		public string ServiceName { get; set; }

		/// <summary>
		/// Navigation to service currencies.
		/// </summary>
		public ICollection<CSCurrencies> CSCurrencies { get; set; }
	}
}
