namespace GreatCurrency.Web.ViewModels
{
    public class BestRatesViewModel
    {
        /// <summary>
        /// Best Currency Id.
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
        /// Bank Id.
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// Bank Name.
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Request time.
        /// </summary>
        public DateTime RequestTime { get; set; }
    }
}
