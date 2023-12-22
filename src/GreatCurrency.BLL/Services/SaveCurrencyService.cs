using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
    /// <inheritdoc cref="ISaveCurrencyService"/>
    public class SaveCurrencyService : ISaveCurrencyService
    {
        private readonly ICityService _cityService;
        private readonly IBankService _bankService;
        private readonly IBankDepartmentService _bankDepartmentService;
        private readonly ICurrencyService _currencyService;
        private readonly IBestCurrencyService _bestCurrencyService;
        private readonly IRequestService _requestService;

        public SaveCurrencyService(
            ICityService cityService,
            IBankService bankService,
            IBankDepartmentService bankDepartmentService,
            ICurrencyService currencyService,
            IBestCurrencyService bestCurrencyService,
            IRequestService requestService
            )
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _bankDepartmentService = bankDepartmentService ?? throw new ArgumentNullException(nameof(bankDepartmentService));
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
        }

        public async Task GetAndSaveAsync()
        {

            GetCurrencyService getCurrencyService = new GetCurrencyService();

            var cities = await _cityService.GetAllCitiesAsync();
            if (cities.Any())
            {
                foreach (var city in cities)
                {
                    if (city.CityURL != null)
                    {
                        var currencies = await getCurrencyService.GetCurrencyAsync(city.CityURL);
                        var newRequest = new RequestDto
                        {
                            IncomingDate = DateTime.Now
                        };

                        List<BestCurrencyDto> tableForCount = [];

                        var requestId = await _requestService.AddRequestAsync(newRequest);

                        foreach (var currency in currencies)
                        {
                            var bankId = await BankCheckOrSaveAsync(currency.BankName);
                            var departmentId = await DepartmentChekOrSaveAsync(currency.FilialName, bankId, city.Id);

                            var newCurrency = new CurrencyDto
                            {
                                USDBuyRate = currency.USDBuyRate,
                                USDSaleRate = currency.USDSaleRate,
                                EURBuyRate = currency.EURBuyRate,
                                EURSaleRate = currency.EURSaleRate,
                                RUBBuyRate = currency.RUBBuyRate,
                                RUBSaleRate = currency.RUBSaleRate,
                                BankDepartmentId = departmentId,
                                RequestId = requestId
                            };

                            tableForCount.Add(new BestCurrencyDto
                            {
                                USDBuyRate = currency.USDBuyRate,
                                USDSaleRate = currency.USDSaleRate,
                                EURBuyRate = currency.EURBuyRate,
                                EURSaleRate = currency.EURSaleRate,
                                RUBBuyRate = currency.RUBBuyRate,
                                RUBSaleRate = currency.RUBSaleRate,
                                BankId = bankId,
                                CityId = city.Id,
                                RequestId = requestId
                            });

                            await _currencyService.AddCurrencyAsync(newCurrency);
                        }

                        await SaveBestCurrencyAsync(tableForCount);
                    }
                }
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
                    BankId = bankId,
                    CityId = bankcurrencies.Select(cityid=> cityid.CityId).First(),
                    RequestId = bankcurrencies.Select(r => r.RequestId).FirstOrDefault()
                };

                await _bestCurrencyService.AddCurrencyAsync(bestBankCurrency);
            }
        }
    }
}
