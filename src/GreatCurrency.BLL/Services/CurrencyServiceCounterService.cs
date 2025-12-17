using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="ICurrencyServiceCounterService"/>
	public class CurrencyServiceCounterService(ICSCurrencyService cSCurrencyService) : ICurrencyServiceCounterService
	{
		private readonly ICSCurrencyService _cSCurrencyService = cSCurrencyService ?? throw new ArgumentNullException(nameof(cSCurrencyService));
		private record RateType(string Name, Func<CSCurrencyDto, double> Selector, bool IsMax);

		public async Task<CSStatisticDto> CurrencyServiceCounterAsync(DateTime startDate, DateTime endDate, int serviceid)
		{
			var allCurrencies = await _cSCurrencyService.GetCurrenciesByTimeAsync(startDate, endDate);

			if (allCurrencies.Count <= 1)
				return null;

			var allTime = allCurrencies[^1].RequestTime - allCurrencies[0].RequestTime;

			var grouped = allCurrencies
			.GroupBy(c => c.RequestId)
			.OrderBy(g => g.Key)
			.ToList();

			var configs = new[]
		{
			new RateType("USD Buy",  x => x.USDBuyRate,  true),
			new RateType("USD Sell", x => x.USDSaleRate, false),
			new RateType("EUR Buy",  x => x.EURBuyRate,  true),
			new RateType("EUR Sell", x => x.EURSaleRate, false),
			new RateType("RUB Buy",  x => x.RUBBuyRate,  true),
			new RateType("RUB Sell", x => x.RUBSaleRate, false),
			new RateType("EURUSD Buy", x => x.EURUSDBuyRate, true),
			new RateType("EURUSD Sell", x => x.EURUSDSellRate, false),
			new RateType("USDRUB Buy", x => x.USDRUBBuyRate, true),
			new RateType("USDRUB Sell", x => x.USDRUBSellRate, false),
			new RateType("EURRUB Buy", x => x.EURRUBBuyRate, true),
			new RateType("EURRUB Sell", x => x.EURRUBSellRate, false)
		};

			var bestByType = configs.ToDictionary(
			cfg => cfg.Name,
			cfg => grouped
				.Select(g => GetBest(g, cfg, serviceid))
				.ToList()
		);

			if (grouped.Count == 1)
			{
				return new CSStatisticDto
				{
					ServiceId = serviceid,
					USDBuyStatistic = bestByType["USD Buy"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestUsdBuyRates = [],
					USDSellStatistic = bestByType["USD Sell"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestUsdSellRates = [],
					EURBuyStatistic = bestByType["EUR Buy"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestEURBuyRates = [],
					EURSellStatistic = bestByType["EUR Sell"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestEURSellRates = [],
					RUBBuyStatistic = bestByType["RUB Buy"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestRubBuyRates = [],
					RUBSellStatistic = bestByType["RUB Sell"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestRubSellRates = [],
					EURUSDBuyStatistic = bestByType["EURUSD Buy"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestEURUSDBuyRates = [],
					EURUSDSellStatistic = bestByType["EURUSD Sell"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestEURUSDSellRates = [],
					USDRUBBuyStatistic = bestByType["USDRUB Buy"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestUSDRUBBuyRates = [],
					USDRUBSellStatistic = bestByType["USDRUB Sell"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestUSDRUBSellRates = [],
					EURRUBBuyStatistic = bestByType["EURRUB Buy"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestEURRUBBuyRates = [],
					EURRUBSellStatistic = bestByType["EURRUB Sell"][0].CurrencyServiceId == serviceid ? 100 : 0,
					BestEURRUBSellRates = []
				};
			}

			var intervals = bestByType.ToDictionary(
			kv => kv.Key,
			kv => GetTimeIntervals(kv.Value, serviceid)
			);

			return new CSStatisticDto
			{
				ServiceId = serviceid,

				USDBuyStatistic = CountStatistic.CountStatic(intervals["USD Buy"], allTime),
				BestUsdBuyRates = intervals["USD Buy"],

				USDSellStatistic = CountStatistic.CountStatic(intervals["USD Sell"], allTime),
				BestUsdSellRates = intervals["USD Sell"],

				EURBuyStatistic = CountStatistic.CountStatic(intervals["EUR Buy"], allTime),
				BestEURBuyRates = intervals["EUR Buy"],

				EURSellStatistic = CountStatistic.CountStatic(intervals["EUR Sell"], allTime),
				BestEURSellRates = intervals["EUR Sell"],

				RUBBuyStatistic = CountStatistic.CountStatic(intervals["RUB Buy"], allTime),
				BestRubBuyRates = intervals["RUB Buy"],

				RUBSellStatistic = CountStatistic.CountStatic(intervals["RUB Sell"], allTime),
				BestRubSellRates = intervals["RUB Sell"],

				EURUSDBuyStatistic = CountStatistic.CountStatic(intervals["EURUSD Buy"], allTime),
				BestEURUSDBuyRates = intervals["EURUSD Buy"],

				EURUSDSellStatistic = CountStatistic.CountStatic(intervals["EURUSD Sell"], allTime),
				BestEURUSDSellRates = intervals["EURUSD Sell"],

				USDRUBBuyStatistic = CountStatistic.CountStatic(intervals["USDRUB Buy"], allTime),
				BestUSDRUBBuyRates = intervals["USDRUB Buy"],

				USDRUBSellStatistic = CountStatistic.CountStatic(intervals["USDRUB Sell"], allTime),
				BestUSDRUBSellRates = intervals["USDRUB Sell"],

				EURRUBBuyStatistic = CountStatistic.CountStatic(intervals["EURRUB Buy"], allTime),
				BestEURRUBBuyRates = intervals["EURRUB Buy"],

				EURRUBSellStatistic = CountStatistic.CountStatic(intervals["EURRUB Sell"], allTime),
				BestEURRUBSellRates = intervals["EURRUB Sell"]
			};
		}

		/// <summary>
		/// Chek is there service in list with best rates
		/// </summary>
		/// <param name="group">List currencies</param>
		/// <param name="cfg">Rate type sell or buy</param>
		/// <param name="serviceid">Service id</param>
		/// <returns>Best currency dto model our service or random service.<returns>
		private static CSCurrencyDto GetBest(
		  IGrouping<int, CSCurrencyDto> group,
		  RateType cfg,
		  int serviceid)
		{
			var bestValue = cfg.IsMax
				? group.Max(cfg.Selector)
				: group.Min(cfg.Selector);

			var list = group.Where(c => cfg.Selector(c) == bestValue).ToList();

			return list.FirstOrDefault(c => c.CurrencyServiceId == serviceid) ?? list[0];
		}

		/// <summary>
		/// Get times when our service was the best.
		/// </summary>
		/// <param name="list">List of Best rates</param>
		/// <param name="serviceid">Service Id</param>
		/// <returns>List of time periods</returns>
		private static List<TimeRates> GetTimeIntervals(List<CSCurrencyDto> list, int serviceid)
		{
			var result = new List<TimeRates>();
			if (list.Count == 0)
				return result;

			var ordered = list.OrderBy(x => x.RequestTime).ToList();

			CSCurrencyDto start = null;

			foreach (var item in ordered)
			{
				if (item.CurrencyServiceId == serviceid)
				{
					start ??= item;
				}
				else if (start != null)
				{
					result.Add(new TimeRates
					{
						startTime = start.RequestTime,
						endTime = item.RequestTime
					});
					start = null;
				}
			}

			if (start != null)
			{
				result.Add(new TimeRates
				{
					startTime = start.RequestTime,
					endTime = ordered[^1].RequestTime
				});
			}

			return result;
		}
	}
}
