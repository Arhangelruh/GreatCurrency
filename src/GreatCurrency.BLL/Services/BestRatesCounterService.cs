using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="IBestRatesCounterService"/>
	public class BestRatesCounterService(IBestCurrencyService bestCurrencyService) : IBestRatesCounterService
	{
		private readonly IBestCurrencyService _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));

		private record RateType(string Name, Func<BestCurrencyDto, double> Selector, bool IsMax);

		public async Task<RateStatisticDto> BestCurrencyCounterAsync(DateTime startDate, DateTime endDate, int bankId, int cityId)
		{
			var allCurrencies = await _bestCurrencyService.GetCurrenciesByTimeAsync(startDate, endDate, cityId);

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
				.Select(g => GetBest(g, cfg, bankId))
				.ToList()
		);

			if (grouped.Count == 1)
			{
				return new RateStatisticDto
				{
					BankId = bankId,
					USDBuyStatistic = bestByType["USD Buy"][0].BankId == bankId ? 100 : 0,
					bestUsdBuyRates = [],
					USDSellStatistic = bestByType["USD Sell"][0].BankId == bankId ? 100 : 0,
					bestUsdSellRates = [],
					EURBuyStatistic = bestByType["EUR Buy"][0].BankId == bankId ? 100 : 0,
					bestEURBuyRates = [],
					EURSellStatistic = bestByType["EUR Sell"][0].BankId == bankId ? 100 : 0,
					bestEURSellRates = [],
					RUBBuyStatistic = bestByType["RUB Buy"][0].BankId == bankId ? 100 : 0,
					bestRubBuyRates = [],
					RUBSellStatistic = bestByType["RUB Sell"][0].BankId == bankId ? 100 : 0,
					bestRubSellRates = [],
					EURUSDBuyStatistic = bestByType["EURUSD Buy"][0].BankId == bankId ? 100 : 0,
					bestEURUSDBuyRates = [],
					EURUSDSellStatistic = bestByType["EURUSD Sell"][0].BankId == bankId ? 100 : 0,
					bestEURUSDSellRates = [],
					USDRUBBuyStatistic = bestByType["USDRUB Buy"][0].BankId == bankId ? 100 : 0,
					bestUSDRUBBuyRates = [],
					USDRUBSellStatistic = bestByType["USDRUB Sell"][0].BankId == bankId ? 100 : 0,
					bestUSDRUBSellRates = [],
					EURRUBBuyStatistic = bestByType["EURRUB Buy"][0].BankId == bankId ? 100 : 0,
					bestEURRUBBuyRates = [],
					EURRUBSellStatistic = bestByType["EURRUB Sell"][0].BankId == bankId ? 100 : 0,
					bestEURRUBSellRates = []
				};
			}

			var intervals = bestByType.ToDictionary(
			kv => kv.Key,
			kv => GetTimeIntervals(kv.Value, bankId)
		);

			return new RateStatisticDto
			{
				BankId = bankId,

				USDBuyStatistic = CountStatistic.CountStatic(intervals["USD Buy"], allTime),
				bestUsdBuyRates = intervals["USD Buy"],

				USDSellStatistic = CountStatistic.CountStatic(intervals["USD Sell"], allTime),
				bestUsdSellRates = intervals["USD Sell"],

				EURBuyStatistic = CountStatistic.CountStatic(intervals["EUR Buy"], allTime),
				bestEURBuyRates = intervals["EUR Buy"],

				EURSellStatistic = CountStatistic.CountStatic(intervals["EUR Sell"], allTime),
				bestEURSellRates = intervals["EUR Sell"],

				RUBBuyStatistic = CountStatistic.CountStatic(intervals["RUB Buy"], allTime),
				bestRubBuyRates = intervals["RUB Buy"],

				RUBSellStatistic = CountStatistic.CountStatic(intervals["RUB Sell"], allTime),
				bestRubSellRates = intervals["RUB Sell"],

				EURUSDBuyStatistic = CountStatistic.CountStatic(intervals["EURUSD Buy"], allTime),
				bestEURUSDBuyRates = intervals["EURUSD Buy"],

				EURUSDSellStatistic = CountStatistic.CountStatic(intervals["EURUSD Sell"], allTime),
				bestEURUSDSellRates = intervals["EURUSD Sell"],

				USDRUBBuyStatistic = CountStatistic.CountStatic(intervals["USDRUB Buy"], allTime),
				bestUSDRUBBuyRates = intervals["USDRUB Buy"],

				USDRUBSellStatistic = CountStatistic.CountStatic(intervals["USDRUB Sell"], allTime),
				bestUSDRUBSellRates = intervals["USDRUB Sell"],

				EURRUBBuyStatistic = CountStatistic.CountStatic(intervals["EURRUB Buy"], allTime),
				bestEURRUBBuyRates = intervals["EURRUB Buy"],

				EURRUBSellStatistic = CountStatistic.CountStatic(intervals["EURRUB Sell"], allTime),
				bestEURRUBSellRates = intervals["EURRUB Sell"]
			};					
		}

		/// <summary>
		/// Chek is there our bank in list with best rates.
		/// </summary>
		/// <param name="group">List currencies</param>
		/// <param name="cfg">Rate type sell or buy</param>
		/// <param name="bankId">Bank id</param>
		/// <returns>Best currency dto model our bank or random service.<returns>
		private static BestCurrencyDto GetBest(
		IGrouping<int, BestCurrencyDto> group,
		RateType cfg,
		int bankId)
		{
			var bestValue = cfg.IsMax
				? group.Max(cfg.Selector)
				: group.Min(cfg.Selector);

			var list = group.Where(c => cfg.Selector(c) == bestValue).ToList();

			return list.FirstOrDefault(c => c.BankId == bankId) ?? list[0];
		}

		/// <summary>
		/// Get times when our bank was the best.
		/// </summary>
		/// <param name="list">List of Best rates</param>
		/// <param name="bankId">Bank id</param>
		/// <returns>List of time periods</returns>
		private static List<TimeRates> GetTimeIntervals(List<BestCurrencyDto> list, int bankId)
		{
			var result = new List<TimeRates>();
			if (list.Count == 0)
				return result;

			var ordered = list.OrderBy(x => x.RequestTime).ToList();

			BestCurrencyDto start = null;

			foreach (var item in ordered)
			{
				if (item.BankId == bankId)
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
