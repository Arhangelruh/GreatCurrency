using GreatCurrency.DAL.Models;

namespace GreatCurrency.BLL.Models
{
	public class LECurrencyDto
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
		/// CNY buy currency.
		/// </summary>
		public double CNYBuyRate { get; set; }

		/// <summary>
		/// CNY sale currency.
		/// </summary>
		public double CNYSaleRate { get; set; }

		/// <summary>
		/// Navigate to currency service.
		/// </summary>
		public int OrganisationId { get; set; }		

		/// <summary>
		/// Navigate to SC request.
		/// </summary>
		public int RequestId { get; set; }
	}
}
