namespace GreatCurrency.BLL.Models.MyfinModels
{
	public class MyfinAPICurrency
	{
		/// <summary>
		/// Currency Id.
		/// </summary>
		public int id { get; set; }

		/// <summary>
		/// Rate code.
		/// </summary>
		public string code { get; set; }

		/// <summary>
		/// Rate name.
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// Currency value.
		/// </summary>
		public int multiplier { get; set; }
	}
}
