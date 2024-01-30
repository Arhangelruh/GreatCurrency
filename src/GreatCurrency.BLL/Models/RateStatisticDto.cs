namespace GreatCurrency.BLL.Models
{
    public class RateStatisticDto
    {
        /// <summary>
        /// Bank id.
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// USD buy percent static.
        /// </summary>
        public int USDBuyStatistic { get; set; }

        /// <summary>
        /// Times whith best usd buy rates.
        /// </summary>
        public List<TimeRates> bestUsdBuyRates { get; set; }

        /// <summary>
        /// USD sell percent static.
        /// </summary>
        public int USDSellStatistic { get; set; }

        /// <summary>
        /// Times whith best usd sell rates.
        /// </summary>
        public List<TimeRates> bestUsdSellRates { get; set; }

        /// <summary>
        /// EUR Buy percent static.
        /// </summary>
        public int EURBuyStatistic { get; set; }

        /// <summary>
        /// Times whith best eur buy rates.
        /// </summary>
        public List<TimeRates> bestEURBuyRates { get; set; }

        /// <summary>
        /// EUR sell percent static.
        /// </summary>
        public int EURSellStatistic { get; set; }

        /// <summary>
        /// Times whith best eur sell rates.
        /// </summary>
        public List<TimeRates> bestEURSellRates { get; set; }

        /// <summary>
        /// RUB buy percent static.
        /// </summary>
        public int RUBBuyStatistic { get; set; }

        /// <summary>
        /// Times whith best rub buy rates.
        /// </summary>
        public List<TimeRates> bestRubBuyRates { get; set; }

        /// <summary>
        /// RUB sell percent static.
        /// </summary>
        public int RUBSellStatistic { get; set; }

        /// <summary>
        /// Times whith best rub sell rates.
        /// </summary>
        public List<TimeRates> bestRubSellRates { get; set; }

    }
}
