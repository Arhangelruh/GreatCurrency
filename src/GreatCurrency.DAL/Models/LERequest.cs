namespace GreatCurrency.DAL.Models
{
	public class LERequest
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
		/// Navigation to Currency.
		/// </summary>
		public ICollection<LECurrency> LECurrencies { get; set; }
	}
}
