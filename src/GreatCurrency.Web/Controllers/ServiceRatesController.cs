using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.BLL.Services;
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

		/// <summary>
		/// Get all services.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Services()
		{
			var services = await _serviceCurrencyService.GetAllServicesAsync();
			var models = new List<ServiceViewModel>();

			if (services.Count() != 0)
			{
				foreach (var service in services)
				{
					models.Add(new ServiceViewModel
					{
						Id = service.Id,
						ServiceName = service.ServiceName						
					});
				}
			}
			return View(models);
		}

		/// <summary>
		/// Model for create service.
		/// </summary>
		/// <returns>View</returns>
		[HttpGet]
		public IActionResult CreateService()
		{
			return View();
		}

		/// <summary>
		/// Create request.
		/// </summary>
		/// <param name="model">Service view model</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> CreateService(ServiceViewModel model)
		{
			if (ModelState.IsValid)
			{
				var services = await _serviceCurrencyService.GetAllServicesAsync();
				var checkService = services.FirstOrDefault(service => service.ServiceName == model.ServiceName);

				if (checkService == null)
				{
					var service = new ServiceDto()
					{
						ServiceName = model.ServiceName						
					};

					await _serviceCurrencyService.AddServiceAsync(service);					

					return RedirectToAction("Services");
				}
				else
				{
					ModelState.AddModelError("Error", "Такой сервис уже есть в списке.");
					return View(model);
				}
			}
			return View(model);
		}

		/// <summary>
		/// Model for edit.
		/// </summary>
		/// <param name="serviceId">Service id</param>
		/// <returns>View service model</returns>
		[HttpGet]
		public async Task<IActionResult> EditService(int serviceId)
		{
			var getService = await _serviceCurrencyService.GetServiceByIdAsync(serviceId);
			if (getService != null)
			{
				var serviceViewModel = new ServiceViewModel
				{
					Id = getService.Id,
					ServiceName = getService.ServiceName					
				};
				return View(serviceViewModel);
			}
			else
			{
				ViewBag.ErrorTitle = "Ошибка";
				ViewBag.ErrorMessage = "Сервис не найден.";
				return View("~/Views/Error/NotFound.cshtml");
			}
		}

		/// <summary>
		/// Edit service.
		/// </summary>
		/// <param name="model">Service view model</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> EditService(ServiceViewModel model)
		{
			if (ModelState.IsValid)
			{
				var service = new ServiceDto
				{
					Id = model.Id,
					ServiceName = model.ServiceName					
				};

				await _serviceCurrencyService.UpdateServiceAsync(service);
				return RedirectToAction("Services");
			}
			else
			{
				return View(model.Id);
			}
		}

		/// <summary>
		/// Get service.
		/// </summary>
		/// <param name="serviceId">Service id</param>
		/// <returns>Service view model</returns>
		[HttpGet]
		public async Task<IActionResult> GetService(int serviceId)
		{
			var getservice = await _serviceCurrencyService.GetServiceByIdAsync(serviceId);
			if (getservice != null)
			{
				var service = new ServiceViewModel
				{
					Id = getservice.Id,
					ServiceName = getservice.ServiceName					
				};
				return View(service);
			}
			else
			{
				ViewBag.ErrorTitle = "Ошибка";
				ViewBag.ErrorMessage = "Сервис не найден.";
				return View("~/Views/Error/NotFound.cshtml");
			}
		}

		/// <summary>
		/// Delete service
		/// </summary>
		/// <param name="serviceId"></param>
		/// <returns>Result</returns>
		[HttpGet]
		public async Task<IActionResult> DeleteService(int serviceId)
		{
			var service = await _serviceCurrencyService.GetServiceByIdAsync(serviceId);
			if (service != null)
			{
				var deleteResult = await _serviceCurrencyService.DeleteServiceAsync(service);
				if (deleteResult)
				{
					return Json("success");
				}
				return Json("error");
			}
			return Json("error");
		}		
	}


}
