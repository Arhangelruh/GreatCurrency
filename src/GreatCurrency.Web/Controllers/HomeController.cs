using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IBankDepartmentService _bankDepartmentService;
        private readonly IBankService _bankService;
        private readonly IBestCurrencyService _bestCurrencyService;
        private readonly ICityService _cityService;
        private readonly GetParameters _getParameters;
        private readonly IBestRatesCounterService _bestRatesCounterService;
        private readonly IRequestService _requestService;


        public HomeController(
            ICurrencyService currencyService,
            IBankDepartmentService bankDepartmentService,
            IBankService bankService,
            IBestCurrencyService bestCurrencyService,
            ICityService cityService,
            GetParameters getParameters,
            IBestRatesCounterService bestRatesCounterService,
            IRequestService requestService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            _bankDepartmentService = bankDepartmentService ?? throw new ArgumentNullException(nameof(bankDepartmentService));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
            _bestRatesCounterService = bestRatesCounterService ?? throw new ArgumentNullException(nameof(bestRatesCounterService));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, int? pagesize, RequestViewModel requestData)                
        {
            int pageSize = (int)(pagesize == null ? 30 : pagesize);
            ViewData["PageSize"] = pageSize;

            List<CityViewModel> list = [];

            var cities = await _cityService.GetAllCitiesAsync();
            if (cities.Count != 0) {
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

           // List<BankDto> bankList = [];

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

            var bestRates = await _bestCurrencyService.GetCurrenciesByTimeAsync(firstDate, secondDate,cityId);

            if (!bestRates.Any())
            {
                return View();
            }

            var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate, cityId=cityId, bankId = bankId };
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

                return View((PaginatedList<BestRatesViewModel>.Create(modelsBestRates, page ?? 1, pageSize), statistic, requestViewModel));
            }
            return View((PaginatedList<BestRatesViewModel>.Create(modelsBestRates, page ?? 1, pageSize), requestViewModel));
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


        /// <summary>
        /// Delete currency request.
        /// </summary>
        /// <param name="datatime">datatime parameter</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteDataBestCurrency(DeleteDataViewModel dataTime)
        {
            if (ModelState.IsValid)
            {
                await _bestCurrencyService.DeleteCurrenciesAsync(dataTime.Date);
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
