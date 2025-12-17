namespace GreatCurrency.DAL.Models
{
    public class BestCurrency
    {
        /// <summary>
        /// Currency Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Usd buy currency.
        /// </summary>
        public double USDBuyRate { get; set; }

        /// <summary>
        /// Usd sale currency.
        /// </summary>
        public double USDSaleRate { get; set; }

        /// <summary>
        /// Eur buy currency.
        /// </summary>
        public double EURBuyRate { get; set; }

        /// <summary>
        /// Eur sale currency.
        /// </summary>
        public double EURSaleRate { get; set; }

        /// <summary>
        /// RUB buy currency.
        /// </summary>
        public double RUBBuyRate { get; set; }

        /// <summary>
        /// RUB sale currency.
        /// </summary>
        public double RUBSaleRate { get; set; }

		/// <summary>
		/// EURUSD buy currency.
		/// </summary>
		public double EURUSDBuyRate { get; set; }

		/// <summary>
		/// EURUSD sell currency.
		/// </summary>
		public double EURUSDSellRate { get; set; }

		/// <summary>
		/// USDRUB buy currency.
		/// </summary>
		public double USDRUBBuyRate { get; set; }

		/// <summary>
		/// USDRUB sell currency.
		/// </summary>
		public double USDRUBSellRate { get; set; }

		/// <summary>
		/// EURRUB buy currency.
		/// </summary>
		public double EURRUBBuyRate { get; set; }

		/// <summary>
		/// EURRUB sell currency.
		/// </summary>
		public double EURRUBSellRate { get; set; }

		/// <summary>
		/// Navigate to bank.
		/// </summary>
		public int BankId { get; set; }

        /// <summary>
        /// Navigate to bank.
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// Navigate to request.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Navigate to request.
        /// </summary>
        public Request Request { get; set; }

        /// <summary>
        /// Navigate to city.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Navigate to city.
        /// </summary>
        public City City { get; set; }

    }
}
