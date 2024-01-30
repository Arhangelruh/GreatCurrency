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
