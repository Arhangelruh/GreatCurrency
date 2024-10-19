namespace GreatCurrency.DAL.Models
{
    public class Request
    {
        /// <summary>
        /// Currency Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Incoming date.
        /// </summary>
        public DateTime IncomingDate { get; set; }

        /// <summary>
        /// Navigation to Currency.
        /// </summary>
        public ICollection<Currency> Currencies { get; set; }

        /// <summary>
        /// Navigation to Best Currency.
        /// </summary>
        public ICollection<BestCurrency> BestCurrencies { get; set; }

		/// <summary>
		/// Navigation to services currency.
		/// </summary>
		public ICollection<CSCurrencies> CSCurrencies { get; set; }
	}
}
