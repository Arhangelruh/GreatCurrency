using GreatCurrency.BLL.Interfaces;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
	public class BestRatesController
		(
		ICityService cityService,
		IBankService bankService,
		GetParameters getParameters,
		IBestCurrencyService bestCurrencyService
		) : Controller
	{
		private readonly ICityService _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
		private readonly IBankService _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
		private readonly GetParameters _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
		private readonly IBestCurrencyService _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));

		[HttpGet]
		public async Task<IActionResult> BestRates(int? page, int? pagesize, RequestViewModel requestData)
		{
			int pageSize = (int)(pagesize == null ? 30 : pagesize);
			ViewData["PageSize"] = pageSize;

			List<CityViewModel> list = [];

			var cities = await _cityService.GetAllCitiesAsync();
			if (cities.Count != 0)
			{
				foreach (var city in cities)
				{
					list.Add(new CityViewModel
					{
						Id = city.Id,
						CityName = city.CityName
					});
				}
			}
			else
			{
				return View();
			}
			ViewBag.CityList = list;

			var mainbank = await _bankService.GetBankByNameAsync(_getParameters.MainBank);
			if (mainbank is null)
			{
				ViewBag.ErrorMessage = "Не найден банк по умолчанию указанный в конфигурации.";
				ViewBag.ErrorTitle = "Ошибка";
				return View("~/Views/Error/Error.cshtml");
			}

			var banks = await _bankService.GetAllBanksAsync();

			ViewBag.BankList = banks;

			DateTime now = DateTime.Now;

			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? now.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? now.Date.AddDays(1) : requestData.endDate);
			var cityId = (int)(requestData.cityId == null ? cities.First().Id : requestData.cityId);
			var bankId = (int)(requestData.bankId == null ? mainbank.Id : requestData.bankId);

			ViewData["StartData"] = firstDate;
			ViewData["EndData"] = secondDate;
			ViewData["CurrentCity"] = cityId;
			ViewData["CurrentBank"] = bankId;

			var bestRates = await _bestCurrencyService.GetBestCurrenciesAsync(firstDate, secondDate, cityId, page ?? 0, pageSize);
			if (bestRates.Count == 0)
			{
				return View();
			}
			var getCount = await _bestCurrencyService.BestCurrencyCountsAsync(firstDate, secondDate, cityId);
			var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate, cityId = cityId, bankId = bankId };

			var modelsBestRates = new List<BestRatesViewModel>();

			foreach (var rate in bestRates)
			{

				var bank = await _bankService.GetBankByIdAsync(rate.BankId);

				modelsBestRates.Add(new BestRatesViewModel
				{
					Id = rate.Id,
					USDBuyRate = rate.USDBuyRate,
					USDSaleRate = rate.USDSaleRate,
					EURBuyRate = rate.EURBuyRate,
					EURSaleRate = rate.EURSaleRate,
					RUBBuyRate = rate.RUBBuyRate,
					RUBSaleRate = rate.RUBSaleRate,
					BankId = rate.BankId,
					BankName = bank.BankName,
					RequestTime = rate.RequestTime
				});
			}

			var paginatedList = new PaginatedList<BestRatesViewModel>(modelsBestRates, getCount, page ?? 0, pageSize);
			return View((paginatedList, requestViewModel));
		}
	}
}
