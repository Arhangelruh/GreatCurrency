using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
	public class LegalCurrencyCounterService(ILECurrencyService currencyService) : ILegalCurrencyCounterService
	{
		private readonly ILECurrencyService _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
		public async Task<LegalRatesStatisticDto?> LegalCurrencyCounterAsync(DateTime startDate, DateTime endDate, int id)
		{
			var allCurrencies = await _currencyService.GetCurrenciesByTimeAsync(startDate, endDate);
			if (allCurrencies.Count > 1)
			{
				var allTime = allCurrencies[^1].RequestTime - allCurrencies[0].RequestTime;

				var requests = allCurrencies.Select(c => c.RequestId).Distinct().ToList();

				List<LECurrencyDto> bestUSDBuyRates = [];
				List<LECurrencyDto> bestUSDSellRates = [];
				List<LECurrencyDto> bestEURBuyRates = [];
				List<LECurrencyDto> bestEURSellRates = [];
				List<LECurrencyDto> bestRUBBuyRates = [];
				List<LECurrencyDto> bestRUBSellRates = [];
				List<LECurrencyDto> bestCNYBuyRates = [];
				List<LECurrencyDto> bestCNYSellRates = [];


				foreach (var request in requests)
				{
					var requestRates = allCurrencies.Where(cur => cur.RequestId == request);

					var bestRequestCurrencyUSDBuy = requestRates.Where(cur => cur.USDBuyRate == requestRates.Select(usd => usd.USDBuyRate).Max()).ToList();
					bestUSDBuyRates.Add(SortServiceRates(bestRequestCurrencyUSDBuy, id));

					var reqRatesUSDSaleWithoutNull = requestRates.Where(cur => cur.USDSaleRate != 0);
					if (reqRatesUSDSaleWithoutNull.Count() > 0)
					{
						var bestRequestCurrencyUSDSell = requestRates.Where(cur => cur.USDSaleRate == reqRatesUSDSaleWithoutNull.Select(usd => usd.USDSaleRate).Min()).ToList();
						bestUSDSellRates.Add(SortServiceRates(bestRequestCurrencyUSDSell, id));
					}

					var bestRequestCurrencyEURBuy = requestRates.Where(cur => cur.EURBuyRate == requestRates.Select(eur => eur.EURBuyRate).Max()).ToList();
					bestEURBuyRates.Add(SortServiceRates(bestRequestCurrencyEURBuy, id));

					var reqRatesEURSaleWithoutNull = requestRates.Where(cur => cur.EURSaleRate != 0);
					if (reqRatesEURSaleWithoutNull.Count() > 0)
					{
						var bestRequestCurrencyEURSell = requestRates.Where(cur => cur.EURSaleRate == reqRatesEURSaleWithoutNull.Select(eur => eur.EURSaleRate).Min()).ToList();
						bestEURSellRates.Add(SortServiceRates(bestRequestCurrencyEURSell, id));
					}

					var bestRequestCurrencyRUBBuy = requestRates.Where(cur => cur.RUBBuyRate == requestRates.Select(rub => rub.RUBBuyRate).Max()).ToList();
					bestRUBBuyRates.Add(SortServiceRates(bestRequestCurrencyRUBBuy, id));

					var reqRatesRUBSaleWithoutNull = requestRates.Where(cur => cur.RUBSaleRate != 0);
					if (reqRatesRUBSaleWithoutNull.Count() > 0)
					{
						var bestRequestCurrencyRUBSell = reqRatesRUBSaleWithoutNull.Where(cur => cur.RUBSaleRate == reqRatesRUBSaleWithoutNull.Select(rub => rub.RUBSaleRate).Min()).ToList();
						bestRUBSellRates.Add(SortServiceRates(bestRequestCurrencyRUBSell, id));
					}

					var bestRequestCurrencyCNYBuy = requestRates.Where(cur => cur.CNYBuyRate == requestRates.Select(cny => cny.CNYBuyRate).Max()).ToList();
					bestCNYBuyRates.Add(SortServiceRates(bestRequestCurrencyCNYBuy, id));

					var reqRatesCNYSaleWithoutNull = requestRates.Where(cur => cur.CNYSaleRate != 0);
					if (reqRatesCNYSaleWithoutNull.Count() > 0)
					{
						var bestRequestCurrencyCNYSell = reqRatesCNYSaleWithoutNull.Where(cur => cur.CNYSaleRate == reqRatesCNYSaleWithoutNull.Select(cny => cny.CNYSaleRate).Min()).ToList();
						bestCNYSellRates.Add(SortServiceRates(bestRequestCurrencyCNYSell, id));
					}

				}

				if (requests.Count == 1)
				{
					var currencyModel = new LegalRatesStatisticDto
					{
						OrganisationId = id,
						USDBuyStatistic = bestUSDBuyRates[0].OrganisationId == id ? 100 : 0,
						BestUsdBuyRates = [],
						USDSellStatistic = bestUSDSellRates[0].OrganisationId == id ? 100 : 0,
						BestUsdSellRates = [],
						EURBuyStatistic = bestEURBuyRates[0].OrganisationId == id ? 100 : 0,
						BestEURBuyRates = [],
						EURSellStatistic = bestEURSellRates[0].OrganisationId == id ? 100 : 0,
						BestEURSellRates = [],
						RUBBuyStatistic = bestRUBBuyRates[0].OrganisationId == id ? 100 : 0,
						BestRubBuyRates = [],
						RUBSellStatistic = bestRUBSellRates[0].OrganisationId == id ? 100 : 0,
						BestRubSellRates = [],
						CNYBuyStatistic = bestCNYBuyRates[0].OrganisationId == id ? 100 : 0,
						BestCNYBuyRates = [],
						CNYSellStatistic = bestCNYSellRates[0].OrganisationId == id ? 100 : 0,
						BestCNYSellRates = []
					};
					return currencyModel;
				}

				var USDBuyRates = GetTimeRates(bestUSDBuyRates, id);
				var USDSellRates = GetTimeRates(bestUSDSellRates, id);
				var EURBuyRates = GetTimeRates(bestEURBuyRates, id);
				var EURSellRates = GetTimeRates(bestEURSellRates, id);
				var RUBBuyRates = GetTimeRates(bestRUBBuyRates, id);
				var RUBSellRates = GetTimeRates(bestRUBSellRates, id);
				var CNYBuyRates = GetTimeRates(bestCNYBuyRates, id);
				var CNYSellRates = GetTimeRates(bestCNYSellRates, id);

				return new LegalRatesStatisticDto
				{
					OrganisationId = id,
					USDBuyStatistic = CountStatistic.CountStatic(USDBuyRates, allTime),
					BestUsdBuyRates = USDBuyRates,
					USDSellStatistic = CountStatistic.CountStatic(USDSellRates, allTime),
					BestUsdSellRates = USDSellRates,
					EURBuyStatistic = CountStatistic.CountStatic(EURBuyRates, allTime),
					BestEURBuyRates = EURBuyRates,
					EURSellStatistic = CountStatistic.CountStatic(EURSellRates, allTime),
					BestEURSellRates = EURSellRates,
					RUBBuyStatistic = CountStatistic.CountStatic(RUBBuyRates, allTime),
					BestRubBuyRates = RUBBuyRates,
					RUBSellStatistic = CountStatistic.CountStatic(RUBSellRates, allTime),
					BestRubSellRates = RUBSellRates,
					CNYBuyStatistic = CountStatistic.CountStatic(CNYBuyRates, allTime),
					BestCNYBuyRates = CNYBuyRates,
					CNYSellStatistic = CountStatistic.CountStatic(CNYSellRates, allTime),
					BestCNYSellRates = CNYSellRates
				};
			}
			return null;
		}

		/// <summary>
		/// Chek is there bank in list with best rates
		/// </summary>
		/// <param name="serviceCurrencies">List currencies</param>
		/// <param name="serviceid">Service id</param>
		/// <returns>Best currency dto model need bank or random bank.<returns>
		private static LECurrencyDto SortServiceRates(List<LECurrencyDto> serviceCurrencies, int id)
		{
			var lookRate = serviceCurrencies.FirstOrDefault(cur => cur.OrganisationId == id);
			if (lookRate != null)
			{
				return lookRate;
			}
			else
			{
				return serviceCurrencies.First();
			}
		}

		/// <summary>
		/// Get times when our service used best courses.
		/// </summary>
		/// <param name="list">List of Best rates</param>
		/// <returns>List of time periods</returns>
		private static List<TimeRates> GetTimeRates(List<LECurrencyDto> list, int id)
		{

			List<TimeRates> timeRates = [];

			if (list.Count != 0)
			{
				int counter = 0;
				int requestIDStartTime = 0;

				var ratesByService = list.OrderBy(r => r.RequestTime).ToList();

				for (int i = 0; i <= ratesByService.Count - 1; i++)
				{

					if (counter == 0 && ratesByService[i].OrganisationId == id && i != ratesByService.Count - 1)
					{
						requestIDStartTime = ratesByService[i].RequestId;
						counter++;
					}
					else
					{
						if (ratesByService[i].OrganisationId != id)
						{
							if (counter != 0)
							{
								if (requestIDStartTime != 0)
								{
									timeRates.Add(new TimeRates
									{
										startTime = list.First(c => c.RequestId == requestIDStartTime).RequestTime,
										endTime = ratesByService[i].RequestTime
									});
									counter = 0;
									requestIDStartTime = 0;
								}
							}
						}
						else
						{
							if (i == ratesByService.Count - 1)
							{
								if (requestIDStartTime != 0)
								{
									timeRates.Add(new TimeRates
									{
										startTime = list.First(c => c.RequestId == requestIDStartTime).RequestTime,
										endTime = ratesByService[i].RequestTime
									});
									counter = 0;
								}
							}
						}
					}
				}
			}
			return timeRates;
		}
	}
}
