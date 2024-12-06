using GreatCurrency.BLL.Interfaces;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
	public class HomeController(
		ICurrencyService currencyService,
		IBankDepartmentService bankDepartmentService,
		IBankService bankService,
		IBestCurrencyService bestCurrencyService,
		ICityService cityService,
		GetParameters getParameters,
		IBestRatesCounterService bestRatesCounterService,
		IRequestService requestService,
		ICurrencyServiceCounterService currencyServiceCounterService,
		IServiceCurrencyService serviceCurrencyService
		) : Controller
	{
		private readonly ICurrencyService _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
		private readonly IBankDepartmentService _bankDepartmentService = bankDepartmentService ?? throw new ArgumentNullException(nameof(bankDepartmentService));
		private readonly IBankService _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
		private readonly IBestCurrencyService _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
		private readonly ICityService _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
		private readonly GetParameters _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
		private readonly IBestRatesCounterService _bestRatesCounterService = bestRatesCounterService ?? throw new ArgumentNullException(nameof(bestRatesCounterService));
		private readonly IRequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
		private readonly ICurrencyServiceCounterService _currencyServiceCounterService = currencyServiceCounterService ?? throw new ArgumentNullException(nameof(currencyServiceCounterService));
		private readonly IServiceCurrencyService _serviceCurrencyService = serviceCurrencyService ?? throw new ArgumentNullException(nameof(serviceCurrencyService));

		[HttpGet]
		public async Task<IActionResult> Index(int? page, int? pagesize, RequestViewModel requestData)
		{
			int pageSize = (int)(pagesize == null ? 30 : pagesize);
			ViewData["PageSize"] = pageSize;

			List<CityViewModel> list = [];

			DateTime now = DateTime.Now;
			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? now.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? now.Date.AddDays(1) : requestData.endDate);

			var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate };

			ViewData["StartData"] = firstDate;
			ViewData["EndData"] = secondDate;

			FullStatisticViewModel model = new();

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

				ViewBag.CityList = list;

				var mainbank = await _bankService.GetBankByNameAsync(_getParameters.MainBank);

				var cityId = (int)(requestData.cityId == null ? cities.First().Id : requestData.cityId);

				requestViewModel.cityId = cityId;

				var banks = await _bankService.GetAllBanksAsync();

				if (banks.Count != 0)
				{
					ViewData["CurrentCity"] = cityId;

					mainbank = mainbank != null ? mainbank : banks.FirstOrDefault();
					ViewBag.BankList = banks;
					var bankId = (int)(requestData.bankId == null ? mainbank.Id : requestData.bankId);

					requestViewModel.bankId = bankId;

					ViewData["CurrentBank"] = bankId;

					var getStatistic = await _bestRatesCounterService.BestCurrencyCounterAsync(firstDate, secondDate, (int)bankId, cityId);

					if (getStatistic != null)
					{
						var bankForStatistic = await _bankService.GetBankByIdAsync(getStatistic.BankId);
						var statistic = new StatisticViewModel
						{
							BankName = bankForStatistic.BankName,
							USDBuyStatistic = getStatistic.USDBuyStatistic,
							bestUsdBuyRates = getStatistic.bestUsdBuyRates,
							USDSellStatistic = getStatistic.USDSellStatistic,
							bestUsdSellRates = getStatistic.bestUsdSellRates,
							EURBuyStatistic = getStatistic.EURBuyStatistic,
							bestEURBuyRates = getStatistic.bestEURBuyRates,
							EURSellStatistic = getStatistic.EURSellStatistic,
							bestEURSellRates = getStatistic.bestEURSellRates,
							RUBBuyStatistic = getStatistic.RUBBuyStatistic,
							bestRubBuyRates = getStatistic.bestRubBuyRates,
							RUBSellStatistic = getStatistic.RUBSellStatistic,
							bestRubSellRates = getStatistic.bestRubSellRates
						};
						model.Statistic = statistic;
					}
				}
			}

			var mainService = await _serviceCurrencyService.GetServiceByNameAsync(_getParameters.MainService);
			var services = await _serviceCurrencyService.GetAllServicesAsync();


			if (services.Count > 0)
			{
				mainService = mainService != null ? mainService : services.FirstOrDefault();

				List<ServiceViewModel> serviceList = [];

				foreach (var service in services)
				{
					serviceList.Add(new ServiceViewModel
					{
						Id = service.Id,
						ServiceName = service.ServiceName
					});
				}

				ViewBag.ServiceList = serviceList;

				var serviceId = (int)(requestData.ServiceId == null ? mainService.Id : requestData.ServiceId);
				requestViewModel.ServiceId = serviceId;

				var getServiceStatistic = await _currencyServiceCounterService.CurrencyServiceCounterAsync(firstDate, secondDate, serviceId);

				if (getServiceStatistic != null)
				{
					var serviceForStatistic = await _serviceCurrencyService.GetServiceByIdAsync(serviceId);

					var serviceStatistic = new ServiceStatisticViewModel
					{
						ServiceName = serviceForStatistic.ServiceName,
						USDBuyStatistic = getServiceStatistic.USDBuyStatistic,
						bestUsdBuyRates = getServiceStatistic.BestUsdBuyRates,
						USDSellStatistic = getServiceStatistic.USDSellStatistic,
						bestUsdSellRates = getServiceStatistic.BestUsdSellRates,
						EURBuyStatistic = getServiceStatistic.EURBuyStatistic,
						bestEURBuyRates = getServiceStatistic.BestEURBuyRates,
						EURSellStatistic = getServiceStatistic.EURSellStatistic,
						bestEURSellRates = getServiceStatistic.BestEURSellRates,
						RUBBuyStatistic = getServiceStatistic.RUBBuyStatistic,
						bestRubBuyRates = getServiceStatistic.BestRubBuyRates,
						RUBSellStatistic = getServiceStatistic.RUBSellStatistic,
						bestRubSellRates = getServiceStatistic.BestRubSellRates
					};
					model.ServiceStatistic = serviceStatistic;
				}
			}

			model.Request = requestViewModel;
			return View(model);
		}

		/// <summary>
		/// Model for delete data.
		/// </summary>
		/// <returns>View</returns>
		[HttpGet]
		public IActionResult DeleteData()
		{
			return View();
		}

		/// <summary>
		/// Delete currency request.
		/// </summary>
		/// <param name="datatime">datatime parameter</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> DeleteData(DeleteDataViewModel dataTime)
		{
			if (ModelState.IsValid)
			{
				await _currencyService.DeleteCurrenciesAsync(dataTime.Date);
				var banks = await _bankService.GetAllBanksAsync();
				foreach (var bank in banks)
				{
					var departments = await _bankDepartmentService.GetAllBankDepartmentsAsync(bank);
					foreach (var department in departments)
					{
						await _bankDepartmentService.DeleteBankDepartmentAsync(department);
					}

					await _bankService.DeleteBankAsync(bank);
				}
				return View("DeleteDataSucces");
			}
			ViewBag.ErrorTitle = "Ошибка";
			ViewBag.ErrorMessage = "Ошибка ввода даты.";
			return View("~/Views/Error/Error.cshtml");
		}		
	}
}
