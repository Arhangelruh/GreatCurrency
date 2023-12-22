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
    }
}
