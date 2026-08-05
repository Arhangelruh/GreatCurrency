namespace GreatCurrency.Web.Models
{
	public class LineChart
	{
		/// <summary>
		/// Currency name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// List of rates.
		/// </summary>
		public List<LineRate> List { get; set; }
	}
}
