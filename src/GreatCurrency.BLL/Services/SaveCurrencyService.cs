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

        public SaveCurrencyService(ICityService cityService, IBankService bankService, IBankDepartmentService bankDepartmentService, ICurrencyService currencyService)
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _bankDepartmentService = bankDepartmentService ?? throw new ArgumentNullException(nameof(bankDepartmentService));
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }

        public async Task GetAndSaveAsync()
        {

            GetCurrencyService getCurrencyService = new GetCurrencyService();

            var cities = await _cityService.GetAllCitiesAsync();
            if (cities.Any())
            {
                foreach (var city in cities)
                {
                    var currencies = await getCurrencyService.GetCurrencyAsync(city.CityURL);
                    foreach (var currency in currencies)
                    {
                        var bank = await BankCheckOrSaveAsync(currency.BankName);
                        var department = await DepartmentChekOrSaveAsync(currency.FilialName, bank, city.Id);

                        var newCurrency = new CurrencyDto
                        {
                            USDBuyRate = currency.USDBuyRate,
                            USDSaleRate = currency.USDSaleRate,
                            EURBuyRate = currency.EURBuyRate,
                            EURSaleRate = currency.EURSaleRate,
                            RUBBuyRate = currency.RUBBuyRate,
                            RUBSaleRate = currency.RUBSaleRate,
                            BankDepartmentId = department.Id
                        };

                        await _currencyService.AddCurrencyAsync(newCurrency);
                    }
                }
            }
        }

        public async Task<BankDto> BankCheckOrSaveAsync(string bankName)
        {
            var getBank = await _bankService.GetBankByNameAsync(bankName);
            if (getBank != null)
            {
                var bank = new BankDto
                {
                    Id = getBank.Id,
                    BankName = bankName
                };
                return bank;
            }
            else
            {
                var addBank = new BankDto { BankName = bankName };
                await _bankService.AddBankAsync(addBank);
                var bank = await _bankService.GetBankByNameAsync(bankName);
                return bank;
            }
        }

        public async Task<BankDepartmentDto> DepartmentChekOrSaveAsync(string departmentName, BankDto bank, int cityId)
        {
            var getDepartment = await _bankDepartmentService.GetBankDepartmentByNameAsync(departmentName);
            if (getDepartment != null)
            {
                var department = new BankDepartmentDto
                {
                    Id = getDepartment.Id,
                    DepartmentAddress = departmentName, 
                    CityId = getDepartment.CityId,
                    BankId = getDepartment.BankId
                };
                return department;
            }
            else
            {
                var addDepartment = new BankDepartmentDto { 
                    DepartmentAddress = departmentName,
                    BankId = bank.Id,
                    CityId = cityId
                };
                await _bankDepartmentService.AddBankDepartmentAsync(addDepartment);
                var department = await _bankDepartmentService.GetBankDepartmentByNameAsync(departmentName);
                return department;
            }
        }
    }
}
