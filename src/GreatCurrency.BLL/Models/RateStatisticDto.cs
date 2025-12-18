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

		/// <summary>
		/// EUR/USD buy converting percent static.
		/// </summary>
		public int EURUSDBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best buy converting rate EUR/USD.
		/// </summary>
		public List<TimeRates> bestEURUSDBuyRates { get; set; }

		/// <summary>
		/// EUR/USD sell converting percent static.
		/// </summary>
		public int EURUSDSellStatistic { get; set; }

		/// <summary>
		/// Times whith best sell converting rate EUR/USD.
		/// </summary>
		public List<TimeRates> bestEURUSDSellRates { get; set; }

		/// <summary>
		/// USD/RUB buy converting percent static.
		/// </summary>
		public int USDRUBBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best buy converting rate USD/RUB.
		/// </summary>
		public List<TimeRates> bestUSDRUBBuyRates { get; set; }

		/// <summary>
		/// USD/RUB sell converting percent static.
		/// </summary>
		public int USDRUBSellStatistic { get; set; }

		/// <summary>
		/// Times whith best sell converting rate USD/RUB.
		/// </summary>
		public List<TimeRates> bestUSDRUBSellRates { get; set; }

		/// <summary>
		/// EUR/RUB buy converting percent static.
		/// </summary>
		public int EURRUBBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best buy converting rate EUR/RUB.
		/// </summary>
		public List<TimeRates> bestEURRUBBuyRates { get; set; }

		/// <summary>
		/// EUR/RUB sell converting percent static.
		/// </summary>
		public int EURRUBSellStatistic { get; set; }

		/// <summary>
		/// Times whith best sell converting rate EUR/RUB.
		/// </summary>
		public List<TimeRates> bestEURRUBSellRates { get; set; }

	}
}
