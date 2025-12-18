using GreatCurrency.BLL.Models;

namespace GreatCurrency.Web.ViewModels
{
	public class StatisticViewModel
	{
		/// <summary>
		/// Bank Name.
		/// </summary>
		public string BankName { get; set; }

		/// <summary>
		/// USD buy percent statistic.
		/// </summary>
		public int USDBuyStatistic { get; set; }

		/// <summary>
		/// Best usd buy rates times.
		/// </summary>
		public List<TimeRates> bestUsdBuyRates { get; set; }

		/// <summary>
		/// USD sell percent statistic.
		/// </summary>
		public int USDSellStatistic { get; set; }

		/// <summary>
		/// Best usd sell rates times.
		/// </summary>
		public List<TimeRates> bestUsdSellRates { get; set; }

		/// <summary>
		/// EUR buy percent statistic.
		/// </summary>
		public int EURBuyStatistic { get; set; }

		/// <summary>
		/// Best eur buy rates times.
		/// </summary>
		public List<TimeRates> bestEURBuyRates { get; set; }

		/// <summary>
		/// EUR sell percent statistic.
		/// </summary>
		public int EURSellStatistic { get; set; }

		/// <summary>
		/// Best eur sell rates times.
		/// </summary>
		public List<TimeRates> bestEURSellRates { get; set; }

		/// <summary>
		/// RUB buy percent statistic.
		/// </summary>
		public int RUBBuyStatistic { get; set; }

		/// <summary>
		/// Best rub buy rates times.
		/// </summary>
		public List<TimeRates> bestRubBuyRates { get; set; }

		/// <summary>
		/// RUB sell percent statistic.
		/// </summary>
		public int RUBSellStatistic { get; set; }

		/// <summary>
		/// Best rub sell rates times.
		/// </summary>
		public List<TimeRates> bestRubSellRates { get; set; }

		/// <summary>
		/// EUR/USD buy rate percent statistic.
		/// </summary>
		public int EURUSDBuyStatistic { get; set; }

		/// <summary>
		/// Best EUR/USD buy converting rates times.
		/// </summary>
		public List<TimeRates> bestEURUSDBuyRates { get; set; }

		/// <summary>
		/// USD/EUR rate percent statistic.
		/// </summary>
		public int EURUSDSellStatistic { get; set; }

		/// <summary>
		/// Best EUR/USD sell converting rates times.
		/// </summary>
		public List<TimeRates> bestEURUSDSellRates { get; set; }

		/// <summary>
		/// USD/RUB sell rate percent statistic.
		/// </summary>
		public int USDRUBBuyStatistic { get; set; }

		/// <summary>
		/// Best USD/RUB buy converting rates times.
		/// </summary>
		public List<TimeRates> bestUSDRUBBuyRates { get; set; }

		/// <summary>
		/// USD/RUB sell rate percent statistic.
		/// </summary>
		public int USDRUBSellStatistic { get; set; }

		/// <summary>
		/// Best USD/RUB sell converting rates times.
		/// </summary>
		public List<TimeRates> bestUSDRUBSellRates { get; set; }

		/// <summary>
		/// EUR/RUB buy rate percent statistic.
		/// </summary>
		public int EURRUBBuyStatistic { get; set; }

		/// <summary>
		/// Best EUR/RUB buy converting rates times.
		/// </summary>
		public List<TimeRates> bestEURRUBBuyRates { get; set; }

		/// <summary>
		/// EUR/RUB sell rate percent statistic.
		/// </summary>
		public int EURRUBSellStatistic { get; set; }

		/// <summary>
		/// Best EUR/RUB sell converting rates times.
		/// </summary>
		public List<TimeRates> bestEURRUBSellRates { get; set; }
	}
}
