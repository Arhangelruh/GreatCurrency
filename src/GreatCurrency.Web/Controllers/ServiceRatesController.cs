using GreatCurrency.BLL.Interfaces;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
	public class ServiceRatesController(
		ICSCurrencyService cSCurrencyService,
		IServiceCurrencyService serviceCurrencyService
		) : Controller
	{
		private readonly ICSCurrencyService _cSCurrencyService = cSCurrencyService ?? throw new ArgumentNullException(nameof(cSCurrencyService));
		private readonly IServiceCurrencyService _serviceCurrencyService = serviceCurrencyService ?? throw new ArgumentNullException(nameof(serviceCurrencyService));

		/// <summary>
		/// Get Service currencies.
		/// </summary>
		/// <param name="page">page num</param>
		/// <param name="pagesize">page size</param>
		/// <param name="requestData">request view model (data params)</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> ServiceRates(int? page, int? pagesize, RequestViewModel requestData)
		{
			int pageSize = (int)(pagesize == null ? 30 : pagesize);
			ViewData["PageSize"] = pageSize;

			DateTime now = DateTime.Now;

			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? now.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? now.Date.AddDays(1) : requestData.endDate);

			ViewData["StartData"] = firstDate;
			ViewData["EndData"] = secondDate;

			var serviceRates = await _cSCurrencyService.GetCurrenciesWithParamsAsync(firstDate, secondDate, page ?? 0, pageSize);
			if (serviceRates.Count == 0)
			{
				return View();
			}
			var getCount = await _cSCurrencyService.CurrencyServiceCountsAsync(firstDate, secondDate);
			var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate };

			List<ServiceRatesViewModel> rates = [];

			foreach (var rate in serviceRates)
			{
				var service = await _serviceCurrencyService.GetServiceByIdAsync(rate.CurrencyServiceId);

				rates.Add(new ServiceRatesViewModel
				{
					Id = rate.Id,
					USDBuyRate = rate.USDBuyRate,
					USDSaleRate = rate.USDSaleRate,
					EURBuyRate = rate.EURBuyRate,
					EURSaleRate = rate.EURSaleRate,
					RUBBuyRate = rate.RUBBuyRate,
					RUBSaleRate = rate.RUBSaleRate,
					ServiceId = rate.CurrencyServiceId,
					ServiceName = service.ServiceName ?? "",
					RequestTime = rate.RequestTime
				});
			}

			var paginatedList = new PaginatedList<ServiceRatesViewModel>(rates, getCount, page ?? 0, pageSize);

			return View((paginatedList, requestViewModel));
		}

		[HttpPost]
		public async Task<IActionResult> DeleteServiceCurrencies(DeleteDataViewModel dataTime)
		{

			if (ModelState.IsValid)
			{
				await _cSCurrencyService.DeleteCurrenciesAsync(dataTime.Date);
				await _serviceCurrencyService.CheckOrDeleteAsync();
				return View("~/Views/Home/DeleteDataSucces.cshtml");
			}
			ViewBag.ErrorTitle = "Ошибка";
			ViewBag.ErrorMessage = "Ошибка ввода даты.";
			return View("~/Views/Error/Error.cshtml");
		}
	}


}
