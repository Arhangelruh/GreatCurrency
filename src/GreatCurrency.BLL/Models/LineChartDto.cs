namespace GreatCurrency.BLL.Models
{
	public class LineChartDto
	{
		/// <summary>
		/// Currency name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// List of rates.
		/// </summary>
		public List<LineRateDto> List { get; set; }
	}
}
