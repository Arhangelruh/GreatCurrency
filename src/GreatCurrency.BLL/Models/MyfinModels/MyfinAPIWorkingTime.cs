namespace GreatCurrency.BLL.Models.MyfinModels
{
	public class MyfinAPIWorkingTime
	{
		/// <summary>
		/// Day count number in a week.
		/// </summary>
		public int week_day_number { get; set; }

		/// <summary>
		/// Start working time.
		/// </summary>
		public string start { get; set; }

		/// <summary>
		/// Finish working time.
		/// </summary>
		public string end { get; set; }
	}
}
