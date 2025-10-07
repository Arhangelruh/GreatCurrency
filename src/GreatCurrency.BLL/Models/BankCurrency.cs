namespace GreatCurrency.BLL.Models
{
    public class BankCurrency
    {
        /// <summary>
        /// Bank Name.
        /// </summary>
        public string BankName {  get; set; }

        /// <summary>
        /// Filial Name.
        /// </summary>
        public string FilialName { get; set; }

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
		/// CNY buy currency.
		/// </summary>
		public double CNYBuyRate { get; set; }

		/// <summary>
		/// CNY buy currency.
		/// </summary>
		public double CNYSaleRate { get; set; }
	}
}
