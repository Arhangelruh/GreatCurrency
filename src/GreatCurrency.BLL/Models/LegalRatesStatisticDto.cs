namespace GreatCurrency.BLL.Models
{
	public class LegalRatesStatisticDto
	{
		/// <summary>
		/// Organisation id.
		/// </summary>
		public int OrganisationId { get; set; }

		/// <summary>
		/// USD buy percent static.
		/// </summary>
		public int USDBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best usd buy rates.
		/// </summary>
		public List<TimeRates> BestUsdBuyRates { get; set; }

		/// <summary>
		/// USD sell percent static.
		/// </summary>
		public int USDSellStatistic { get; set; }

		/// <summary>
		/// Times whith best usd sell rates.
		/// </summary>
		public List<TimeRates> BestUsdSellRates { get; set; }

		/// <summary>
		/// EUR Buy percent static.
		/// </summary>
		public int EURBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best eur buy rates.
		/// </summary>
		public List<TimeRates> BestEURBuyRates { get; set; }

		/// <summary>
		/// EUR sell percent static.
		/// </summary>
		public int EURSellStatistic { get; set; }

		/// <summary>
		/// Times whith best eur sell rates.
		/// </summary>
		public List<TimeRates> BestEURSellRates { get; set; }

		/// <summary>
		/// RUB buy percent static.
		/// </summary>
		public int RUBBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best rub buy rates.
		/// </summary>
		public List<TimeRates> BestRubBuyRates { get; set; }

		/// <summary>
		/// RUB sell percent static.
		/// </summary>
		public int RUBSellStatistic { get; set; }

		/// <summary>
		/// Times whith best rub sell rates.
		/// </summary>
		public List<TimeRates> BestRubSellRates { get; set; }

		/// <summary>
		/// CNY buy percent static.
		/// </summary>
		public int CNYBuyStatistic { get; set; }

		/// <summary>
		/// Times whith best rub buy rates.
		/// </summary>
		public List<TimeRates> BestCNYBuyRates { get; set; }

		/// <summary>
		/// CNY sell percent static.
		/// </summary>
		public int CNYSellStatistic { get; set; }

		/// <summary>
		/// Times whith best rub sell rates.
		/// </summary>
		public List<TimeRates> BestCNYSellRates { get; set; }
	}
}
