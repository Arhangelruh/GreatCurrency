namespace GreatCurrency.BLL.Models
{
	public class CSStatisticDto
	{
		/// <summary>
		/// Service id.
		/// </summary>
		public int ServiceId { get; set; }

		/// <summary>
		/// USD buy percent static.
		/// </summary>
		public int USDBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best usd buy rates.
		/// </summary>
		public List<TimeRates> BestUsdBuyRates { get; set; }

		/// <summary>
		/// USD sell percent static.
		/// </summary>
		public int USDSellStatistic { get; set; }

		/// <summary>
		/// Times whith best usd sell rates.
		/// </summary>
		public List<TimeRates> BestUsdSellRates { get; set; }

		/// <summary>
		/// EUR Buy percent static.
		/// </summary>
		public int EURBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best eur buy rates.
		/// </summary>
		public List<TimeRates> BestEURBuyRates { get; set; }

		/// <summary>
		/// EUR sell percent static.
		/// </summary>
		public int EURSellStatistic { get; set; }

		/// <summary>
		/// Times whith best eur sell rates.
		/// </summary>
		public List<TimeRates> BestEURSellRates { get; set; }

		/// <summary>
		/// RUB buy percent static.
		/// </summary>
		public int RUBBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best rub buy rates.
		/// </summary>
		public List<TimeRates> BestRubBuyRates { get; set; }

		/// <summary>
		/// RUB sell percent static.
		/// </summary>
		public int RUBSellStatistic { get; set; }

		/// <summary>
		/// Times whith best rub sell rates.
		/// </summary>
		public List<TimeRates> BestRubSellRates { get; set; }

		/// <summary>
		/// EUR/USD buy converting percent static.
		/// </summary>
		public int EURUSDBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best buy converting rate EUR/USD.
		/// </summary>
		public List<TimeRates> BestEURUSDBuyRates { get; set; }

		/// <summary>
		/// EUR/USD sell converting percent static.
		/// </summary>
		public int EURUSDSellStatistic { get; set; }

		/// <summary>
		/// Times whith best sell converting rate EUR/USD.
		/// </summary>
		public List<TimeRates> BestEURUSDSellRates { get; set; }

		/// <summary>
		/// USD/RUB buy converting percent static.
		/// </summary>
		public int USDRUBBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best buy converting rate USD/RUB.
		/// </summary>
		public List<TimeRates> BestUSDRUBBuyRates { get; set; }

		/// <summary>
		/// USD/RUB sell converting percent static.
		/// </summary>
		public int USDRUBSellStatistic { get; set; }

		/// <summary>
		/// Times whith best sell converting rate USD/RUB.
		/// </summary>
		public List<TimeRates> BestUSDRUBSellRates { get; set; }

		/// <summary>
		/// EUR/RUB byu converting percent static.
		/// </summary>
		public int EURRUBBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best buy converting rate EUR/RUB.
		/// </summary>
		public List<TimeRates> BestEURRUBBuyRates { get; set; }

		/// <summary>
		/// EUR/RUB sell converting percent static.
		/// </summary>
		public int EURRUBSellStatistic { get; set; }

		/// <summary>
		/// Times whith best sell converting rate EUR/RUB.
		/// </summary>
		public List<TimeRates> BestEURRUBSellRates { get; set; }
	}
}
