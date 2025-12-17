using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.BLL.Models.MyfinModels;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="ISaveMyfinAPICurrencyService"/>
	public class SaveMyfinAPICurrencyService : ISaveMyfinAPICurrencyService
	{
		private readonly ICityService _cityService;
		private readonly IMyfinAPIService _myfinAPIService;
		private readonly IServiceCurrencyService _serviceCurrencyService;
		private readonly ISCRequestService _sCRequestService;
		private readonly ICSCurrencyService _cSCurrencyService;
		private readonly IRequestService _requestService;
		private readonly IBankService _bankService;
		private readonly ICurrencyService _currencyService;
		private readonly ICheckCurrency _checkCurrency;
		private readonly IBankDepartmentService _bankDepartmentService;
		private readonly IBestCurrencyService _bestCurrencyService;

		public SaveMyfinAPICurrencyService(
			ICityService cityService,
			IMyfinAPIService myfinAPIService,
			IServiceCurrencyService serviceCurrencyService,
			ISCRequestService sCRequestService,
			ICSCurrencyService cSCurrencyService,
			IRequestService requestService,
			IBankService bankService,
			ICurrencyService currencyService,
			ICheckCurrency checkCurrency,
			IBankDepartmentService bankDepartmentService,
			IBestCurrencyService bestCurrencyService)
		{
			_cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
			_myfinAPIService = myfinAPIService ?? throw new ArgumentNullException(nameof(myfinAPIService));
			_serviceCurrencyService = serviceCurrencyService ?? throw new ArgumentNullException(nameof(serviceCurrencyService));
			_sCRequestService = sCRequestService ?? throw new ArgumentNullException(nameof(sCRequestService));
			_cSCurrencyService = cSCurrencyService ?? throw new ArgumentNullException(nameof(cSCurrencyService));
			_requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
			_bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
			_currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
			_checkCurrency = checkCurrency ?? throw new ArgumentNullException(nameof(checkCurrency));
			_bankDepartmentService = bankDepartmentService ?? throw new ArgumentNullException(nameof(bankDepartmentService));
			_bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
		}

		public async Task GetAndSaveAsync(int mainBankId, string login, string password)
		{
			var cities = await _cityService.GetAllCitiesAsync();
			if (cities.Count != 0)
			{
				var authorize = await _myfinAPIService.GetBearerToken(login, password);

				var myfinCities = await _myfinAPIService.GetAllCities(authorize.token);

				foreach (var city in cities)
				{
					var checkCity = myfinCities.FirstOrDefault(c => c.name == city.CityName);

					if (checkCity != null)
					{
						var currencies = await _myfinAPIService.GetRateCurrenciesByCity(authorize.token, checkCity.id);

						var services = await _serviceCurrencyService.GetAllServicesAsync();

						if (services.Count != 0)
						{

							var servicesCurrencies = new List<MyfinAPICurrencyRates>();

							foreach (var service in services)
							{
								var serviceCurrency = currencies.FirstOrDefault(sc => sc.department_name == service.ServiceName);
								if (serviceCurrency != null)
								{
									servicesCurrencies.Add(serviceCurrency);
									currencies.Remove(serviceCurrency);
								}
							}

							if (servicesCurrencies.Count != 0)
							{
								var sCRequest = new SCRequestDto { IncomingDate = DateTime.Now };
								var sCRequestId = await _sCRequestService.AddRequestAsync(sCRequest);

								foreach (var currency in servicesCurrencies)
								{
									var serviceModel = await _serviceCurrencyService.GetServiceByNameAsync(currency.department_name);

									var USDrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.USD);
									var EURrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.EUR);
									var RUBrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.RUB);
									var EURUSDrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.EURUSD);
									var USDRUBrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.USDRUB);
									var EURRUBrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.EURRUB);

									var EURUSDBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy;
									var EURUSDSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell;
									var USDRUBBuyRate = USDRUBrates == null ? 0 : USDRUBrates.rate_buy;
									var USDRUBSellRate = USDRUBrates == null ? 0 : USDRUBrates.rate_sell;
									var EURRUBBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy;
									var EURRUBSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell;

									var newSCCurrency = new CSCurrencyDto
									{
										RequestId = sCRequestId,
										CurrencyServiceId = serviceModel.Id,
										USDBuyRate = USDrates.rate_buy,
										USDSaleRate = USDrates.rate_sell,
										EURBuyRate = EURrates.rate_buy,
										EURSaleRate = EURrates.rate_sell,
										RUBBuyRate = RUBrates.rate_buy,
										RUBSaleRate = RUBrates.rate_sell,
										EURUSDBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy,
										EURUSDSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell,
										USDRUBBuyRate = USDRUBrates == null ? 0 : USDRUBrates.rate_buy,
										USDRUBSellRate = USDRUBrates == null ? 0 : USDRUBrates.rate_sell,
										EURRUBBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy,
										EURRUBSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell
									};

									await _cSCurrencyService.AddCurrencyAsync(newSCCurrency);
								}
							}
						}

						var newRequest = new RequestDto
						{
							IncomingDate = DateTime.Now
						};

						List<BestCurrencyDto> tableForCount = [];

						var requestId = await _requestService.AddRequestAsync(newRequest);

						foreach (var currency in currencies)
						{
							var bankId = await BankCheckOrSaveAsync(currency.bank.name);
							var departmentId = await DepartmentChekOrSaveAsync(currency.department_name.Replace("\\", ""), bankId, city.Id);

							var USDrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.USD);
							var EURrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.EUR);
							var RUBrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.RUB);
							var EURUSDrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.EURUSD);
							var USDRUBrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.USDRUB);
							var EURRUBrates = currency.rates.FirstOrDefault(c => c.currency.code == RatesConstant.EURRUB);

							var newCurrency = new CurrencyDto
							{
								USDBuyRate = USDrates.rate_buy,
								USDSaleRate = USDrates.rate_sell,
								EURBuyRate = EURrates.rate_buy,
								EURSaleRate = EURrates.rate_sell,
								RUBBuyRate = RUBrates.rate_buy,
								RUBSaleRate = RUBrates.rate_sell,
								EURUSDBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy,
								EURUSDSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell,
								USDRUBBuyRate = USDRUBrates == null ? 0 : USDRUBrates.rate_buy,
								USDRUBSellRate = USDRUBrates == null ? 0 : USDRUBrates.rate_sell,
								EURRUBBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy,
								EURRUBSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell,
								BankDepartmentId = departmentId,
								RequestId = requestId
							};

							tableForCount.Add(new BestCurrencyDto
							{
								USDBuyRate = USDrates.rate_buy,
								USDSaleRate = USDrates.rate_sell,
								EURBuyRate = EURrates.rate_buy,
								EURSaleRate = EURrates.rate_sell,
								RUBBuyRate = RUBrates.rate_buy,
								RUBSaleRate = RUBrates.rate_sell,
								EURUSDBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy,
								EURUSDSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell,
								USDRUBBuyRate = USDRUBrates == null ? 0 : USDRUBrates.rate_buy,
								USDRUBSellRate = USDRUBrates == null ? 0 : USDRUBrates.rate_sell,
								EURRUBBuyRate = EURUSDrates == null ? 0 : EURUSDrates.rate_buy,
								EURRUBSellRate = EURUSDrates == null ? 0 : EURUSDrates.rate_sell,
								BankId = bankId,
								CityId = city.Id,
								RequestId = requestId
							});
							await _currencyService.AddCurrencyAsync(newCurrency);
						}
						await SaveBestCurrencyAsync(tableForCount);
					}
				}
				await _checkCurrency.CheckCurrencyAsync(mainBankId);
			}
		}

		public async Task<int> BankCheckOrSaveAsync(string bankName)
		{
			var getBank = await _bankService.GetBankByNameAsync(bankName);
			if (getBank != null)
			{
				return getBank.Id;
			}
			else
			{
				var addBank = new BankDto { BankName = bankName };
				var bankId = await _bankService.AddBankAsync(addBank);

				return bankId;
			}
		}

		public async Task<int> DepartmentChekOrSaveAsync(string departmentName, int bankId, int cityId)
		{
			var getDepartment = await _bankDepartmentService.GetBankDepartmentByNameAsync(departmentName);
			if (getDepartment != null)
			{
				return getDepartment.Id;
			}
			else
			{
				var addDepartment = new BankDepartmentDto
				{
					DepartmentAddress = departmentName,
					BankId = bankId,
					CityId = cityId
				};
				var departmentId = await _bankDepartmentService.AddBankDepartmentAsync(addDepartment);

				return departmentId;
			}
		}

		public async Task SaveBestCurrencyAsync(List<BestCurrencyDto> listcurrencies)
		{
			var tableIds = listcurrencies.Select(c => c.BankId).Distinct().ToList();

			foreach (var bankId in tableIds)
			{
				var bankcurrencies = listcurrencies.Where(bank => bank.BankId == bankId);

				var bestBankCurrency = new BestCurrencyDto
				{
					USDBuyRate = bankcurrencies.Select(usd => usd.USDBuyRate).Max(),
					USDSaleRate = bankcurrencies.Select(usd => usd.USDSaleRate).Min(),
					EURBuyRate = bankcurrencies.Select(usd => usd.EURBuyRate).Max(),
					EURSaleRate = bankcurrencies.Select(usd => usd.EURSaleRate).Min(),
					RUBBuyRate = bankcurrencies.Select(usd => usd.RUBBuyRate).Max(),
					RUBSaleRate = bankcurrencies.Select(usd => usd.RUBSaleRate).Min(),
					EURUSDBuyRate = bankcurrencies.Select(eurusd => eurusd.EURUSDBuyRate).Max(),
					EURUSDSellRate = bankcurrencies.Select(eurusd => eurusd.EURUSDSellRate).Min(),
					USDRUBBuyRate = bankcurrencies.Select(eurusd => eurusd.USDRUBBuyRate).Max(),
					USDRUBSellRate = bankcurrencies.Select(eurusd => eurusd.USDRUBSellRate).Min(),
					EURRUBBuyRate = bankcurrencies.Select(eurusd => eurusd.EURRUBBuyRate).Max(),
					EURRUBSellRate = bankcurrencies.Select(eurusd => eurusd.EURRUBSellRate).Min(),
					BankId = bankId,
					CityId = bankcurrencies.Select(cityid => cityid.CityId).First(),
					RequestId = bankcurrencies.Select(r => r.RequestId).FirstOrDefault()
				};

				await _bestCurrencyService.AddCurrencyAsync(bestBankCurrency);
			}
		}
	}
}
