namespace GreatCurrency.BLL.Models.MyfinModels
{
	public class MyfinAPICurrencyRates
	{
		/// <summary>
		/// MyfinAPICity model.
		/// </summary>
		public MyfinAPICity city { get; set; }

		/// <summary>
		/// MyfinAPIBank model.
		/// </summary>
		public MyfinAPIBank bank { get; set; }

		/// <summary>
		/// Department id.
		/// </summary>
		public int department_id { get; set; }

		/// <summary>
		/// Department name.
		/// </summary>
		public string department_name { get; set; }

		/// <summary>
		/// Department address.
		/// </summary>
		public string department_address { get; set; }

		/// <summary>
		/// MyfinAPIDepartmentGeo model.
		/// </summary>
		public MyfinAPIDepartmentGeo department_geo { get; set; }

		/// <summary>
		/// Last update time.
		/// </summary>
		public int time_update { get; set; }

		/// <summary>
		/// Rates list.
		/// </summary>
		public List<MyfinAPIRate> rates { get; set; }

		/// <summary>
		/// Working time string view.
		/// </summary>
		public string working_time_string { get; set; }

		/// <summary>
		/// Working time, list MyfinAPIWorkingTime models .
		/// </summary>
		public List<MyfinAPIWorkingTime> working_time { get; set; }
	}
}
