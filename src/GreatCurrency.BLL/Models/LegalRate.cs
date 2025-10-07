namespace GreatCurrency.BLL.Models
{
	public class LegalRate
	{
		/// <summary>
		/// Organisation name.
		/// </summary>
		public string Organisation { get; set; }

		/// <summary>
		/// Currency name.
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Rate value.
		/// </summary>
		public decimal Rate { get; set; }

		/// <summary>
		/// Rate sell or buy meaning.
		/// </summary>
		public string Meaning { get; set; }
	}
}
