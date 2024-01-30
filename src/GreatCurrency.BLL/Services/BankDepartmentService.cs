using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
    public class BankDepartmentService : IBankDepartmentService
    {
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<BankDepartment> _bankDepartmentRepository;

        public BankDepartmentService(IRepository<Bank> bankRepository, IRepository<City> cityRepository, 
            IRepository<BankDepartment> bankDepartmentRepository, IRepository<Currency> currencyRepository)
        {
            _bankRepository = bankRepository ?? throw new ArgumentNullException(nameof(bankRepository));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            _bankDepartmentRepository = bankDepartmentRepository ?? throw new ArgumentNullException(nameof(bankDepartmentRepository));
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
        }

        public async Task<int> AddBankDepartmentAsync(BankDepartmentDto bankDepartmentDto)
        {
            if (bankDepartmentDto is null)
            {
                throw new ArgumentNullException(nameof(bankDepartmentDto));
            }

            var newBankDepartment = new BankDepartment
            {
                DepartmentAddress = bankDepartmentDto.DepartmentAddress,
                BankId = bankDepartmentDto.BankId,
                CityId = bankDepartmentDto.CityId
            };
            await _bankDepartmentRepository.AddAsync(newBankDepartment);
            await _bankDepartmentRepository.SaveChangesAsync();
            
            return newBankDepartment.Id;
        }

        public async Task<bool> DeleteBankDepartmentAsync(BankDepartmentDto bankDepartmentDto)
        {
            if (bankDepartmentDto is null)
            {
                throw new ArgumentNullException(nameof(bankDepartmentDto));
            }

            var currency = await _currencyRepository.GetEntityAsync(currency => currency.BankDepartmentId == bankDepartmentDto.Id);
            if (currency is null)
            {
                var bankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.Id == bankDepartmentDto.Id);
                _bankDepartmentRepository.Delete(bankDepartment);
                await _bankDepartmentRepository.SaveChangesAsync(); 
                return true;
            }
            return false;
        }

        public async Task<List<BankDepartmentDto>> GetAllBankDepartmentsAsync(BankDto bankDto)
        {
            List<BankDepartmentDto> departments = [];

            var getBankDepartments = await _bankDepartmentRepository
                .GetAll()
                .AsNoTracking()
                .Where(bankdepartment => bankdepartment.BankId == bankDto.Id)
                .ToListAsync();

            foreach (var bankDepartment in getBankDepartments)
            {
                departments.Add(new BankDepartmentDto
                {
                    Id = bankDepartment.Id,
                    BankId = bankDepartment.BankId,
                    CityId = bankDepartment.CityId,
                    DepartmentAddress = bankDepartment.DepartmentAddress
                });
            }
            return departments;
        }

        public async Task<List<BankDepartmentDto>> GetAllCityBankDepartmentsAsync(CityDto cityDto)
        {
            List<BankDepartmentDto> departments = [];

            var getBankDepartments = await _bankDepartmentRepository
                .GetAll()
                .AsNoTracking()
                .Where(bankdepartment => bankdepartment.CityId == cityDto.Id)
                .ToListAsync();

            foreach (var bankDepartment in getBankDepartments)
            {
                departments.Add(new BankDepartmentDto
                {
                    Id = bankDepartment.Id,
                    BankId = bankDepartment.BankId,
                    CityId = bankDepartment.CityId,
                    DepartmentAddress = bankDepartment.DepartmentAddress
                });
            }
            return departments;
        }

        public async Task<BankDepartmentDto> GetBankDepartmentByIdAsync(int bankDepartmentDto)
        {
            var getBankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.Id == bankDepartmentDto);
            if (getBankDepartment is null)
            {
                return null;
            }
            var departmentDto = new BankDepartmentDto
            {
                Id = getBankDepartment.Id,
                BankId = getBankDepartment.BankId,
                CityId = getBankDepartment.CityId,
                DepartmentAddress = getBankDepartment.DepartmentAddress
            };

            return departmentDto;
        }

        public async Task<BankDepartmentDto> GetBankDepartmentByNameAsync(string bankDepartmentName)
        {
            var getBankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.DepartmentAddress == bankDepartmentName);
            if (getBankDepartment is null)
            {
                return null;
            }
            var departmentDto = new BankDepartmentDto
            {
                Id = getBankDepartment.Id,
                BankId = getBankDepartment.BankId,
                CityId = getBankDepartment.CityId,
                DepartmentAddress = getBankDepartment.DepartmentAddress
            };

            return departmentDto;
        }
    }
}
