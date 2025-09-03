namespace GreatCurrency.DAL.Models
{
	public class LEOrganisation
	{
		/// <summary>
		/// Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Organisation Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Navigation to legal entities Currency.
		/// </summary>
		public ICollection<LECurrency> LECurrencies { get; set; }
	}
}
