using GreatCurrency.BLL.Interfaces;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GreatCurrency.Web.Controllers
{
	public class BestRatesController
		(
		ICityService cityService,
		IBankService bankService,
		GetParameters getParameters,
		IBestCurrencyService bestCurrencyService,
		IBankDepartmentService bankDepartmentService
		) : Controller
	{
		private readonly ICityService _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
		private readonly IBankService _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
		private readonly GetParameters _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
		private readonly IBestCurrencyService _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
		private readonly IBankDepartmentService _bankDepartmentService = bankDepartmentService ?? throw new ArgumentNullException(nameof(bankDepartmentService));


		/// <summary>
		/// Get best currencies.
		/// </summary>
		/// <param name="page">page num</param>
		/// <param name="pagesize">page size</param>
		/// <param name="requestData">request view model, datatime params</param>
		/// <returns></returns>
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

			var banks = await _bankService.GetAllBanksAsync();

			ViewBag.BankList = banks;

			DateTime now = DateTime.Now;

			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? now.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? now.Date.AddDays(1) : requestData.endDate);
			var cityId = (int)(requestData.cityId == null ? cities.First().Id : requestData.cityId);			

			ViewData["StartData"] = firstDate;
			ViewData["EndData"] = secondDate;
			ViewData["CurrentCity"] = cityId;

			var bestRates = await _bestCurrencyService.GetBestCurrenciesAsync(firstDate, secondDate, cityId, page ?? 0, pageSize);
			if (bestRates.Count == 0)
			{
				return View();
			}
			var getCount = await _bestCurrencyService.BestCurrencyCountsAsync(firstDate, secondDate, cityId);
			var requestViewModel = new RequestViewModel { startDate = firstDate, endDate = secondDate, cityId = cityId };

			List<BestRatesViewModel> modelsBestRates = [];

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
				return View("~/Views/Home/DeleteDataSucces.cshtml");
			}
			ViewBag.ErrorTitle = "Ошибка";
			ViewBag.ErrorMessage = "Ошибка ввода даты.";
			return View("~/Views/Error/Error.cshtml");
		}
	}
}
