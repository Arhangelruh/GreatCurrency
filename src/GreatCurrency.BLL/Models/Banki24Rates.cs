namespace GreatCurrency.BLL.Models
{
	public class Banki24Rates
	{
		/// <summary>
		/// BankCurrency model.
		/// </summary>
		public BankCurrency? bankCurrency { get; set; }

		/// <summary>
		/// DealStockRates model.
		/// </summary>
		public List<DealStockRateDto>? dealStockRates { get; set; }
	}
}
