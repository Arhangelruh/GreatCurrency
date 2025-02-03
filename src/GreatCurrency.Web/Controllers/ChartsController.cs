using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.BLL.Services;
using GreatCurrency.DAL.Models;
using GreatCurrency.Web.Services;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
	public class ChartsController(
		IChartService chartService,
		IBankService bankService,
		ICityService cityService,
		GetParameters getParameters) : Controller
	{
		private readonly IChartService _chartService = chartService ?? throw new ArgumentNullException(nameof(chartService));
		private readonly IBankService _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
		private readonly GetParameters _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
		private readonly ICityService _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));

		[HttpGet]
		public async Task<IActionResult> GetLineData(RequestViewModel requestData)
		{

			DateTime now = DateTime.Now;
			var myDt = DateTime.SpecifyKind(now, DateTimeKind.Unspecified);
			DateTime firstDate = (DateTime)(!requestData.startDate.HasValue ? myDt.Date : requestData.startDate);
			DateTime secondDate = (DateTime)(!requestData.endDate.HasValue ? myDt.Date.AddDays(1) : requestData.endDate);
			
			int cityId;
			if(requestData.cityId == null)
			{
				var cities = await _cityService.GetAllCitiesAsync();
				if(cities.Count > 0)
				{
					cityId = cities[0].Id;
				}
				else {
					return BadRequest("Не найден город по указанному id");
				}
			}
			else
			{
				cityId = (int)requestData.cityId;
			}

			int bankId;
			if(requestData.bankId == null)
			{
				var mainbank = await _bankService.GetBankByNameAsync(_getParameters.MainBank);
				if (mainbank != null)
				{
					return BadRequest("Не найден банк указанный в конфигурации");
				}
				else {
					bankId = mainbank.Id;
				}
			}
			else
			{
				bankId= (int)requestData.bankId;
			}

			var allCurrencies = await _chartService.GetAllRatesAsync(firstDate, secondDate, cityId, bankId);

			//DateTime dateTime = new DateTime(2025, 01, 30, 10, 00, 00);

			//List<LineChartDto> allCurrencies = new List<LineChartDto>();

			//List<LineRateDto> listUSD =
			//[
			//	new LineRateDto { OurSaleRate = 3.4080, BestSaleRate = 3.4070, Time = dateTime.ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4090, BestSaleRate = 3.4070, Time = dateTime.AddHours(1).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4080, BestSaleRate = 3.4080, Time = dateTime.AddHours(2).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4090, BestSaleRate = 3.4080, Time = dateTime.AddHours(3).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4090, BestSaleRate = 3.4070, Time = dateTime.AddHours(4).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4070, BestSaleRate = 3.4070, Time = dateTime.AddHours(5).ToShortTimeString() },
			//];

			//List<LineRateDto> listEUR =
			//[
			//	new LineRateDto { OurSaleRate = 3.4080, BestSaleRate = 3.4070, Time = dateTime.ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4090, BestSaleRate = 3.4070, Time = dateTime.AddHours(1).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4080, BestSaleRate = 3.4080, Time = dateTime.AddHours(2).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4090, BestSaleRate = 3.4080, Time = dateTime.AddHours(3).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4090, BestSaleRate = 3.4070, Time = dateTime.AddHours(4).ToShortTimeString() },
			//	new LineRateDto { OurSaleRate = 3.4070, BestSaleRate = 3.4070, Time = dateTime.AddHours(5).ToShortTimeString() },
			//];

			//allCurrencies.Add(new LineChartDto { Name = "USDList", List = listUSD });
			//allCurrencies.Add(new LineChartDto { Name = "EURList", List = listEUR });

			return Json(allCurrencies);
		}
	}
}
