using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="ICurrencyServiceCounterService"/>
	public class CurrencyServiceCounterService(ICSCurrencyService cSCurrencyService) : ICurrencyServiceCounterService
	{
		private readonly ICSCurrencyService _cSCurrencyService = cSCurrencyService ?? throw new ArgumentNullException(nameof(cSCurrencyService));

		public async Task<CSStatisticDto> CurrencyServiceCounterAsync(DateTime startDate, DateTime endDate, int serviceid)
		{
			var allCurrencies = await _cSCurrencyService.GetCurrenciesByTimeAsync(startDate, endDate);
			if (allCurrencies.Count > 1)
			{
				var allTime = allCurrencies[allCurrencies.Count - 1].RequestTime - allCurrencies[0].RequestTime;

				var requests = allCurrencies.Select(c => c.RequestId).Distinct().ToList();

				List<CSCurrencyDto> bestUSDBuyRates = [];
				List<CSCurrencyDto> bestUSDSellRates = [];
				List<CSCurrencyDto> bestEURBuyRates = [];
				List<CSCurrencyDto> bestEURSellRates = [];
				List<CSCurrencyDto> bestRUBBuyRates = [];
				List<CSCurrencyDto> bestRUBSellRates = [];

				foreach (var request in requests)
				{
					var requestRates = allCurrencies.Where(cur => cur.RequestId == request);

					var bestRequestCurrencyUSDBuy = requestRates.Where(cur => cur.USDBuyRate == requestRates.Select(usd => usd.USDBuyRate).Max()).ToList();
					bestUSDBuyRates.Add(SortServiceRates(bestRequestCurrencyUSDBuy, serviceid));

					var reqRatesUSDSaleWithoutNull = requestRates.Where(cur => cur.USDSaleRate != 0);
					var bestRequestCurrencyUSDSell = requestRates.Where(cur => cur.USDSaleRate == reqRatesUSDSaleWithoutNull.Select(usd => usd.USDSaleRate).Min()).ToList();
					bestUSDSellRates.Add(SortServiceRates(bestRequestCurrencyUSDSell, serviceid));

					var bestRequestCurrencyEURBuy = requestRates.Where(cur => cur.EURBuyRate == requestRates.Select(eur => eur.EURBuyRate).Max()).ToList();
					bestEURBuyRates.Add(SortServiceRates(bestRequestCurrencyEURBuy, serviceid));

					var reqRatesEURSaleWithoutNull = requestRates.Where(cur => cur.EURSaleRate != 0);
					var bestRequestCurrencyEURSell = requestRates.Where(cur => cur.EURSaleRate == reqRatesEURSaleWithoutNull.Select(eur => eur.EURSaleRate).Min()).ToList();
					bestEURSellRates.Add(SortServiceRates(bestRequestCurrencyEURSell, serviceid));

					var bestRequestCurrencyRUBBuy = requestRates.Where(cur => cur.RUBBuyRate == requestRates.Select(rub => rub.RUBBuyRate).Max()).ToList();
					bestRUBBuyRates.Add(SortServiceRates(bestRequestCurrencyRUBBuy, serviceid));

					var reqRatesRUBSaleWithoutNull = requestRates.Where(cur => cur.RUBSaleRate != 0);
					var bestRequestCurrencyRUBSell = reqRatesRUBSaleWithoutNull.Where(cur => cur.RUBSaleRate == reqRatesRUBSaleWithoutNull.Select(rub => rub.RUBSaleRate).Min()).ToList();
					bestRUBSellRates.Add(SortServiceRates(bestRequestCurrencyRUBSell, serviceid));
				}

				if (requests.Count == 1)
				{
					var currencyModel = new CSStatisticDto
					{
						ServiceId = serviceid,
						USDBuyStatistic = bestUSDBuyRates[0].CurrencyServiceId == serviceid ? 100 : 0,
						BestUsdBuyRates = [],
						USDSellStatistic = bestUSDSellRates[0].CurrencyServiceId == serviceid ? 100 : 0,
						BestUsdSellRates = [],
						EURBuyStatistic = bestEURBuyRates[0].CurrencyServiceId == serviceid ? 100 : 0,
						BestEURBuyRates = [],
						EURSellStatistic = bestEURSellRates[0].CurrencyServiceId == serviceid ? 100 : 0,
						BestEURSellRates = [],
						RUBBuyStatistic = bestRUBBuyRates[0].CurrencyServiceId == serviceid ? 100 : 0,
						BestRubBuyRates = [],
						RUBSellStatistic = bestRUBSellRates[0].CurrencyServiceId == serviceid ? 100 : 0,
						BestRubSellRates = []
					};
					return currencyModel;
				}

				var USDBuyRates = GetTimeRates(bestUSDBuyRates, serviceid);
				var USDSellRates = GetTimeRates(bestUSDSellRates, serviceid);
				var EURBuyRates = GetTimeRates(bestEURBuyRates, serviceid);
				var EURSellRates = GetTimeRates(bestEURSellRates, serviceid);
				var RUBBuyRates = GetTimeRates(bestRUBBuyRates, serviceid);
				var RUBSellRates = GetTimeRates(bestRUBSellRates, serviceid);

				return new CSStatisticDto
				{
					ServiceId = serviceid,
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
					BestRubSellRates = RUBSellRates
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
		private static CSCurrencyDto SortServiceRates(List<CSCurrencyDto> serviceCurrencies, int serviceid)
		{
			var lookRate = serviceCurrencies.FirstOrDefault(cur => cur.CurrencyServiceId == serviceid);
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
		private static List<TimeRates> GetTimeRates(List<CSCurrencyDto> list, int serviceid)
		{

			List<TimeRates> timeRates = [];

			if (list.Count != 0)
			{
				int counter = 0;
				int requestIDStartTime = 0;

				var ratesByService = list.OrderBy(r => r.RequestTime).ToList();

				for (int i = 0; i <= ratesByService.Count - 1; i++)
				{

					if (counter == 0 && ratesByService[i].CurrencyServiceId == serviceid && i != ratesByService.Count - 1)
					{
						requestIDStartTime = ratesByService[i].RequestId;
						counter++;
					}
					else
					{
						if (ratesByService[i].CurrencyServiceId != serviceid)
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
