using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
	public class ChartsService(IBestCurrencyService bestCurrencyService, IBankService bankService) : IChartService
	{
		private readonly IBestCurrencyService _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
		private readonly IBankService _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));

		public async Task<List<LineChartDto>> GetAllRatesAsync(DateTime begin, DateTime end, int cityid, int bankId)
		{
			List<LineChartDto> currencies = [];
			var checkBank = await _bankService.GetBankByIdAsync(bankId);
			if (checkBank != null)
			{
				var allCurrencies = await _bestCurrencyService.GetCurrenciesByTimeAsync(begin, end, cityid);
				if (allCurrencies.Count > 0)
				{

					var requests = allCurrencies.Select(c => c.RequestId).Distinct().ToList();

					List<LineRateDto> usdList = [];
					List<LineRateDto> eurList = [];
					List<LineRateDto> rubList = [];

					foreach (var request in requests)
					{
						var requestRates = allCurrencies.Where(cur => cur.RequestId == request);
						var ourRequestCurrency = requestRates.Where(cur => cur.BankId == bankId).ToList();

						var bestRequestCurrencyUSDBuy = requestRates.Where(cur => cur.USDBuyRate == requestRates.Select(usd => usd.USDBuyRate).Max()).ToList();
						var bestRequestCurrencyUSDSell = requestRates.Where(cur => cur.USDSaleRate == requestRates.Select(usd => usd.USDSaleRate).Min()).ToList();

						usdList.Add(new LineRateDto
						{
							BestBuyRate = bestRequestCurrencyUSDBuy.FirstOrDefault().USDBuyRate,
							OurBuyRate = ourRequestCurrency.FirstOrDefault().USDBuyRate,
							BestSaleRate = bestRequestCurrencyUSDSell.FirstOrDefault().USDSaleRate,
							OurSaleRate = ourRequestCurrency.FirstOrDefault().USDSaleRate,
							Time = ourRequestCurrency.FirstOrDefault().RequestTime.ToShortTimeString()
						});

						var bestRequestCurrencyEURBuy = requestRates.Where(cur => cur.EURBuyRate == requestRates.Select(eur => eur.EURBuyRate).Max()).ToList();
						var bestRequestCurrencyEURSell = requestRates.Where(cur => cur.EURSaleRate == requestRates.Select(eur => eur.EURSaleRate).Min()).ToList();
						eurList.Add(new LineRateDto
						{
							BestBuyRate = bestRequestCurrencyEURBuy.FirstOrDefault().EURBuyRate,
							OurBuyRate = ourRequestCurrency.FirstOrDefault().EURBuyRate,
							BestSaleRate = bestRequestCurrencyEURSell.FirstOrDefault().EURSaleRate,
							OurSaleRate = ourRequestCurrency.FirstOrDefault().EURSaleRate,
							Time = ourRequestCurrency.FirstOrDefault().RequestTime.ToShortTimeString()
						});

						var bestRequestCurrencyRUBBuy = requestRates.Where(cur => cur.RUBBuyRate == requestRates.Select(rub => rub.RUBBuyRate).Max()).ToList();
						var bestRequestCurrencyRUBSell = requestRates.Where(cur => cur.RUBSaleRate == requestRates.Select(rub => rub.RUBSaleRate).Min()).ToList();
						rubList.Add(new LineRateDto
						{
							BestBuyRate = bestRequestCurrencyRUBBuy.FirstOrDefault().RUBBuyRate,
							OurBuyRate = ourRequestCurrency.FirstOrDefault().RUBBuyRate,
							BestSaleRate = bestRequestCurrencyRUBSell.FirstOrDefault().RUBSaleRate,
							OurSaleRate = ourRequestCurrency.FirstOrDefault().RUBSaleRate,
							Time = ourRequestCurrency.FirstOrDefault().RequestTime.ToShortTimeString()
						});
					}
					currencies.Add(new LineChartDto { Name = "USDList", List = usdList });
					currencies.Add(new LineChartDto { Name = "EURList", List = eurList });
					currencies.Add(new LineChartDto { Name = "RUBList", List = rubList });

					return currencies;
				}
			}

			return [];
		}
	}
}
