using GreatCurrency.BLL.Interfaces;
using GreatCurrency.Web.Models;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
	public class ChartController(
		IDealStockRatesService dealStockRatesService,
		GetParameters getParameters,
		ILEOrganisationService organisationService,
		ILECurrencyService legalCurrencyService) : Controller
	{
		private readonly IDealStockRatesService _dealStockRatesService = dealStockRatesService ?? throw new ArgumentNullException(nameof(dealStockRatesService));
		private readonly GetParameters _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
		private readonly ILEOrganisationService _organisationService = organisationService ?? throw new ArgumentNullException(nameof(organisationService));
		private readonly ILECurrencyService _legalCurrencyService = legalCurrencyService ?? throw new ArgumentNullException(nameof(legalCurrencyService));

		[HttpGet]
		public async Task<IActionResult> GetStockRates(RequestViewModel requestData)
		{
			DateTime now = DateTime.Now;
			var myDt = DateTime.SpecifyKind(now, DateTimeKind.Unspecified);
			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? myDt.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? myDt.Date.AddDays(1) : requestData.endDate);

			var dealStockRates = await _dealStockRatesService.GetRatesByTimeAsync(firstDate, secondDate);
			var ourOrganisation = await _organisationService.GetOrganisationByNameAsync(_getParameters.MainBank);
			List<LineChart> rates = [];

			if (ourOrganisation != null)
			{

				var allrates = await _legalCurrencyService.GetCurrenciesByTimeAsync(firstDate, secondDate);
				if (allrates.Count != 0)
				{
					var ourRates = allrates.Where(r => r.OrganisationId == ourOrganisation.Id);
					if (ourRates != null)
					{

						List<LineRate> USD = [];
						List<LineRate> EUR = [];
						List<LineRate> CNY = [];
						List<LineRate> RUB = [];

						foreach (var rate in ourRates.OrderBy(r => r.Id))
						{
							var dealRates = dealStockRates.Where(r => r.RequestId == rate.RequestId);

							USD.Add(new LineRate
							{
								OurBuyRate = rate.USDBuyRate,
								OurSellRate = rate.USDSaleRate,
								Time = $"{rate.RequestTime:g}",
								DealRate = dealRates.FirstOrDefault(r => r.Currency == "USD")?.Rate ?? 0
							});

							EUR.Add(new LineRate
							{
								OurBuyRate = rate.EURBuyRate,
								OurSellRate = rate.EURSaleRate,
								Time = $"{rate.RequestTime:g}",
								DealRate = dealRates.FirstOrDefault(r => r.Currency == "EUR")?.Rate ?? 0
							});

							CNY.Add(new LineRate
							{
								OurBuyRate = rate.CNYBuyRate,
								OurSellRate = rate.CNYSaleRate,
								Time = $"{rate.RequestTime:g}",
								DealRate = dealRates.FirstOrDefault(r => r.Currency == "CNY")?.Rate ?? 0
							});

							RUB.Add(new LineRate
							{
								OurBuyRate = rate.RUBBuyRate,
								OurSellRate = rate.RUBSaleRate,
								Time = $"{rate.RequestTime:g}",
								DealRate = dealRates.FirstOrDefault(r => r.Currency == "RUB")?.Rate ?? 0
							});
						}

						rates.AddRange(
							new LineChart { Name = "USD", List = USD },
							new LineChart { Name = "EUR", List = EUR },
							new LineChart { Name = "RUB", List = RUB },
							new LineChart { Name = "CNY", List = CNY }
						);
					}
				}
			}
			return Json(rates);
		}
	}
}
