namespace GreatCurrency.DAL.Models
{
	public class SCRequest
	{
		/// <summary>
		/// Currency Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Incoming date.
		/// </summary>
		public DateTime IncomingDate { get; set; }

		/// <summary>
		/// Navigation to services currency.
		/// </summary>
		public ICollection<CSCurrency> CSCurrencies { get; set; }
	}
}
