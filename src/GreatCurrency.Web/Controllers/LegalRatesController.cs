using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Services;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
	public class LegalRatesController(
		ILEOrganisationService organisationService,
		GetParameters getParameters,
		ILegalCurrencyCounterService legalCurrencyCounterService,
		ILECurrencyService legalCurrencyService
		) : Controller
	{
		private readonly ILEOrganisationService _organisationService = organisationService ?? throw new ArgumentNullException(nameof(organisationService));
		private readonly GetParameters _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
		private readonly ILegalCurrencyCounterService _legalCurrencyCounterService = legalCurrencyCounterService ?? throw new ArgumentNullException(nameof(legalCurrencyCounterService));
		private readonly ILECurrencyService _legalCurrencyService = legalCurrencyService ?? throw new ArgumentNullException(nameof(legalCurrencyService));

		/// <summary>
		/// Get statistic for legal rates/
		/// </summary>
		/// <param name="requestData">Model with data for statistic</param>
		/// <returns>View model</returns>
		public async Task<IActionResult> LegalRates(RequestViewModel requestData)
		{

			DateTime now = DateTime.Now;
			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? now.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? now.Date.AddDays(1) : requestData.endDate);

			var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate };

			ViewData["StartData"] = firstDate;
			ViewData["EndData"] = secondDate;

			var mainOrganisation = await _organisationService.GetOrganisationByNameAsync(_getParameters.MainBank);
			var organisations = await _organisationService.GetAllOrganisationsAsync();

			LegalStatisticViewModel model = new();

			if (organisations.Count != 0)
			{
				mainOrganisation = mainOrganisation != null ? mainOrganisation : organisations.FirstOrDefault();
				ViewBag.OrganisationList = organisations;
				var OrganisationId = (int)(requestData.OrganisationId == null ? mainOrganisation.Id : requestData.OrganisationId);

				requestViewModel.OrganisationId = OrganisationId;				
				var getStatistic = await _legalCurrencyCounterService.LegalCurrencyCounterAsync(firstDate, secondDate, (int)OrganisationId);

				if (getStatistic != null)
				{
					var organisationForStatistic = organisations.FirstOrDefault(org => org.Id == getStatistic.OrganisationId);
					if (organisationForStatistic != null)
					{
						model.Statistic = new LegalRatesStatisticViewModel
						{
							OrganisationName = organisationForStatistic.Name,
							USDBuyStatistic = getStatistic.USDBuyStatistic,
							bestUsdBuyRates = getStatistic.BestUsdBuyRates,
							USDSellStatistic = getStatistic.USDSellStatistic,
							bestUsdSellRates = getStatistic.BestUsdSellRates,
							EURBuyStatistic = getStatistic.EURBuyStatistic,
							bestEURBuyRates = getStatistic.BestEURBuyRates,
							EURSellStatistic = getStatistic.EURSellStatistic,
							bestEURSellRates = getStatistic.BestEURSellRates,
							RUBBuyStatistic = getStatistic.RUBBuyStatistic,
							bestRubBuyRates = getStatistic.BestRubBuyRates,
							RUBSellStatistic = getStatistic.RUBSellStatistic,
							bestRubSellRates = getStatistic.BestRubSellRates,
							CNYSellStatistic = getStatistic.CNYSellStatistic,
							bestCNYSellRates = getStatistic.BestCNYSellRates,
							CNYBuyStatistic = getStatistic.CNYBuyStatistic,
							bestCNYBuyRates = getStatistic.BestCNYBuyRates,							
						};												
					}
					else
					{
						ViewBag.ErrorTitle = "Ошибка";
						ViewBag.ErrorMessage = "Не найдена организация для построения статистики.";
						return View("~/Views/Error/Error.cshtml");
					}
				}
			}
			model.Request = requestViewModel;
			return View(model);
		}

		/// <summary>
		/// Get rates.
		/// </summary>
		/// <param name="page">page num</param>
		/// <param name="pagesize">page size</param>
		/// <param name="requestData">request view model, datatime params</param>
		/// <returns>paginated list for rates</returns>
		[HttpGet]
		public async Task<IActionResult> Rates(int? page, int? pagesize, RequestViewModel requestData)
		{
			int pageSize = (int)(pagesize == null ? 30 : pagesize);
			ViewData["PageSize"] = pageSize;			

			var organisations = await _organisationService.GetAllOrganisationsAsync();

			ViewBag.OrganisationList = organisations;

			DateTime now = DateTime.Now;

			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? now.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? now.Date.AddDays(1) : requestData.endDate);			

			ViewData["StartData"] = firstDate;
			ViewData["EndData"] = secondDate;

			var rates = await _legalCurrencyService.GetCurrenciesAsync(firstDate, secondDate, page ?? 0, pageSize);
			if (rates.Count == 0)
			{
				return View();
			}
			var getCount = await _legalCurrencyService.CurrencyCountAsync(firstDate, secondDate);
			var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate};

			List<LegalRatesViewModel> modelsRates = [];

			foreach (var rate in rates)
			{				
				var organisation = organisations.FirstOrDefault(organisation => organisation.Id == rate.OrganisationId);

				if (organisation != null)
				{
					modelsRates.Add(new LegalRatesViewModel
					{
						Id = rate.Id,
						USDBuyRate = rate.USDBuyRate,
						USDSaleRate = rate.USDSaleRate,
						EURBuyRate = rate.EURBuyRate,
						EURSaleRate = rate.EURSaleRate,
						RUBBuyRate = rate.RUBBuyRate,
						RUBSaleRate = rate.RUBSaleRate,
						CNYBuyRate = rate.CNYBuyRate,
						CNYSaleRate = rate.CNYSaleRate,
						OrganisationId = rate.OrganisationId,
						OrganisationName = organisation.Name,
						RequestTime = rate.RequestTime
					});
				}
				else {
					ViewBag.ErrorTitle = "Ошибка";
					ViewBag.ErrorMessage = "Не найдена организация.";
					return View("~/Views/Error/Error.cshtml");
				}
			}

			var paginatedList = new PaginatedList<LegalRatesViewModel>(modelsRates, getCount, page ?? 0, pageSize);
			return View((paginatedList, requestViewModel));
		}

		/// <summary>
		/// Delete currencies.
		/// </summary>
		/// <param name="dataTime"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> DeleteLegalRates(DeleteDataViewModel dataTime)
		{
			if (ModelState.IsValid)
			{
				await _legalCurrencyService.DeleteCurrenciesAsync(dataTime.Date);
				await _organisationService.CheckOrDeleteAsync();
				return View("~/Views/Home/DeleteDataSucces.cshtml");
			}
			ViewBag.ErrorTitle = "Ошибка";
			ViewBag.ErrorMessage = "Ошибка ввода даты.";
			return View("~/Views/Error/Error.cshtml");
		}
	}
}
