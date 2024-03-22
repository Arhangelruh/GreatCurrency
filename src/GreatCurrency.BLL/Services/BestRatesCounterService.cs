using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GreatCurrency.BLL.Services
{
    public class BestRatesCounterService : IBestRatesCounterService
    {
        private IBestCurrencyService _bestCurrencyService;

        public BestRatesCounterService(IBestCurrencyService bestCurrencyService)
        {
            _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));       
        }

        public async Task<RateStatisticDto> BestCurrencyCounterAsync(DateTime startDate, DateTime endDate, int bankId, int cityId)
        {
            var allCurrencies = await _bestCurrencyService.GetCurrenciesByTimeAsync(startDate, endDate, cityId);

            if (allCurrencies.Count > 1)
            {
                var allTime = allCurrencies[allCurrencies.Count - 1].RequestTime - allCurrencies[0].RequestTime;

                var requests = allCurrencies.Select(c => c.RequestId).Distinct().ToList();

                List<BestCurrencyDto> bestUSDBuyRates = [];
                List<BestCurrencyDto> bestUSDSellRates = [];
                List<BestCurrencyDto> bestEURBuyRates = [];
                List<BestCurrencyDto> bestEURSellRates = [];
                List<BestCurrencyDto> bestRUBBuyRates = [];
                List<BestCurrencyDto> bestRUBSellRates = [];

                foreach (var request in requests)
                {
                    var requestRates = allCurrencies.Where(cur => cur.RequestId == request);

                    var bestRequestCurrencyUSDBuy = requestRates.Where(cur => cur.USDBuyRate == requestRates.Select(usd => usd.USDBuyRate).Max()).ToList();
                    bestUSDBuyRates.Add(sortBestRates(bestRequestCurrencyUSDBuy, bankId));

                    var bestRequestCurrencyUSDSell = requestRates.Where(cur => cur.USDSaleRate == requestRates.Select(usd => usd.USDSaleRate).Min()).ToList();
                    bestUSDSellRates.Add(sortBestRates(bestRequestCurrencyUSDSell, bankId));

                    var bestRequestCurrencyEURBuy = requestRates.Where(cur => cur.EURBuyRate == requestRates.Select(eur => eur.EURBuyRate).Max()).ToList();
                    bestEURBuyRates.Add(sortBestRates(bestRequestCurrencyEURBuy, bankId));

                    var bestRequestCurrencyEURSell = requestRates.Where(cur => cur.EURSaleRate == requestRates.Select(eur => eur.EURSaleRate).Min()).ToList();
                    bestEURSellRates.Add(sortBestRates(bestRequestCurrencyEURSell, bankId));

                    var bestRequestCurrencyRUBBuy = requestRates.Where(cur => cur.RUBBuyRate == requestRates.Select(rub => rub.RUBBuyRate).Max()).ToList();
                    bestRUBBuyRates.Add(sortBestRates(bestRequestCurrencyRUBBuy, bankId));

                    var bestRequestCurrencyRUBSell = requestRates.Where(cur => cur.RUBSaleRate == requestRates.Select(rub => rub.RUBSaleRate).Min()).ToList();
                    bestRUBSellRates.Add(sortBestRates(bestRequestCurrencyRUBSell, bankId));
                }

                if (requests.Count == 1)
                {
                    return new RateStatisticDto
                    {
                        BankId = bankId,
                        USDBuyStatistic = bestUSDBuyRates[0].BankId==bankId ? 100 : 0,
                        bestUsdBuyRates = [],
                        USDSellStatistic = bestUSDSellRates[0].BankId == bankId ? 100 : 0,
                        bestUsdSellRates = [],
                        EURBuyStatistic = bestEURBuyRates[0].BankId == bankId ? 100 : 0,
                        bestEURBuyRates = [],
                        EURSellStatistic = bestEURSellRates[0].BankId == bankId ? 100 : 0,
                        bestEURSellRates = [],
                        RUBBuyStatistic = bestRUBBuyRates[0].BankId == bankId ? 100 : 0,
                        bestRubBuyRates = [],
                        RUBSellStatistic = bestRUBSellRates[0].BankId == bankId ? 100 : 0,
                        bestRubSellRates = []
                    };
                }

                var USDBuyRates = GetTimeRates(bestUSDBuyRates, bankId);
                var USDSellRates = GetTimeRates(bestUSDSellRates, bankId);
                var EURBuyRates = GetTimeRates(bestEURBuyRates, bankId);
                var EURSellRates = GetTimeRates(bestEURSellRates, bankId);
                var RUBBuyRates = GetTimeRates(bestRUBBuyRates, bankId);
                var RUBSellRates = GetTimeRates(bestRUBSellRates, bankId);

                return new RateStatisticDto
                {
                    BankId = bankId,
                    USDBuyStatistic = CountStatic(USDBuyRates, allTime),
                    bestUsdBuyRates = USDBuyRates,
                    USDSellStatistic = CountStatic(USDSellRates, allTime),
                    bestUsdSellRates = USDSellRates,
                    EURBuyStatistic = CountStatic(EURBuyRates, allTime),
                    bestEURBuyRates = EURBuyRates,
                    EURSellStatistic = CountStatic(EURSellRates, allTime),
                    bestEURSellRates = EURSellRates,
                    RUBBuyStatistic = CountStatic(RUBBuyRates, allTime),
                    bestRubBuyRates = RUBBuyRates,
                    RUBSellStatistic = CountStatic(RUBSellRates, allTime),
                    bestRubSellRates = RUBSellRates
                };
            }
            return null;
        }

        public BestCurrencyDto sortBestRates(List<BestCurrencyDto> bestCurrencies, int bankId)
        {
            var lookRate = bestCurrencies.FirstOrDefault(cur => cur.BankId == bankId);
            if (lookRate != null)
            {
                return lookRate;
            }
            else
            {
                return bestCurrencies.First();
            }
        }

        public List<TimeRates> GetTimeRates(List<BestCurrencyDto> list, int bankId)
        {

            List<TimeRates> timeRates = [];

            if (list.Any())
            {
                int counter = 0;
                int requestIDStartTime = 0;

                var ratesByBank = list.OrderBy(r => r.RequestTime).ToList();

                for (int i = 0; i<= ratesByBank.Count()-1;i++) {
                    
                    if (counter == 0 && ratesByBank[i].BankId == bankId && i!= ratesByBank.Count() - 1)
                    {
                        requestIDStartTime = ratesByBank[i].RequestId;
                        counter++;
                    }
                    else
                    {
                        if (ratesByBank[i].BankId != bankId)
                        {
                            if (counter != 0)
                            {
                                if (requestIDStartTime != 0)
                                {
                                    timeRates.Add(new TimeRates
                                    {
                                        startTime = list.First(c => c.RequestId == requestIDStartTime).RequestTime,
                                        endTime = ratesByBank[i].RequestTime
                                    });
                                    counter = 0;
                                    requestIDStartTime = 0;
                                }
                            }
                        }
                        else
                        {
                            if (i == ratesByBank.Count() - 1) {
                                if (requestIDStartTime != 0)
                                {
                                    timeRates.Add(new TimeRates
                                    {
                                        startTime = list.First(c => c.RequestId == requestIDStartTime).RequestTime,
                                        endTime = ratesByBank[i].RequestTime
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

        public int CountStatic(List<TimeRates> list, TimeSpan alltime)
        {

            TimeSpan countTime = TimeSpan.Zero;

            foreach (var line in list)
            {
                var lineTime = line.endTime - line.startTime;
                countTime += lineTime;
            }

            var allTimeInMinutes = alltime.TotalMinutes;
            var ourBestTime = countTime.TotalMinutes;

            return (int)Math.Round(ourBestTime * 100 / allTimeInMinutes);

        }
    }
}
