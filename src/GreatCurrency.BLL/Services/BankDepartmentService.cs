using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
    public class BankDepartmentService(IRepository<Bank> bankRepository, IRepository<City> cityRepository,
		IRepository<BankDepartment> bankDepartmentRepository, IRepository<Currency> currencyRepository) : IBankDepartmentService
    {
        private readonly IRepository<Currency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
        private readonly IRepository<BankDepartment> _bankDepartmentRepository = bankDepartmentRepository ?? throw new ArgumentNullException(nameof(bankDepartmentRepository));

		public async Task<int> AddBankDepartmentAsync(BankDepartmentDto bankDepartmentDto)
        {
			ArgumentNullException.ThrowIfNull(bankDepartmentDto);

			var newBankDepartment = new BankDepartment
            {
                DepartmentName = bankDepartmentDto.DepartmentName,
                DepartmentAddress = bankDepartmentDto.DepartmentAddress,
                BankId = bankDepartmentDto.BankId,
                CityId = bankDepartmentDto.CityId,
                ExternalDepartmentId = bankDepartmentDto.ExternalDepartmentId
            };
            await _bankDepartmentRepository.AddAsync(newBankDepartment);
            await _bankDepartmentRepository.SaveChangesAsync();
            
            return newBankDepartment.Id;
        }

        public async Task<bool> DeleteBankDepartmentAsync(BankDepartmentDto bankDepartmentDto)
        {
			ArgumentNullException.ThrowIfNull(bankDepartmentDto);

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
                    DepartmentName = bankDepartment.DepartmentName,
                    DepartmentAddress = bankDepartment.DepartmentAddress,
                    ExternalDepartmentId = bankDepartment.ExternalDepartmentId
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
                    DepartmentName= bankDepartment.DepartmentName,
                    DepartmentAddress = bankDepartment.DepartmentAddress,
                    ExternalDepartmentId= bankDepartment.ExternalDepartmentId
                });
            }
            return departments;
        }

        public async Task<BankDepartmentDto> GetBankDepartmentByIdAsync(int bankDepartmentId)
        {
            var getBankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.Id == bankDepartmentId);
            if (getBankDepartment is null)
            {
                return null;
            }
            var departmentDto = new BankDepartmentDto
            {
                Id = getBankDepartment.Id,
                BankId = getBankDepartment.BankId,
                CityId = getBankDepartment.CityId,
                DepartmentName = getBankDepartment.DepartmentName,
                DepartmentAddress = getBankDepartment.DepartmentAddress,
                ExternalDepartmentId = getBankDepartment.ExternalDepartmentId
            };

            return departmentDto;
        }

        public async Task<BankDepartmentDto> GetBankDepartmentByNameAsync(string bankDepartmentName)
        {
            var getBankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.DepartmentName == bankDepartmentName);
            if (getBankDepartment is null)
            {
                return null;
            }
            var departmentDto = new BankDepartmentDto
            {
                Id = getBankDepartment.Id,
                BankId = getBankDepartment.BankId,
                CityId = getBankDepartment.CityId,
                DepartmentName= getBankDepartment.DepartmentName,
                DepartmentAddress = getBankDepartment.DepartmentAddress,
                ExternalDepartmentId= getBankDepartment.ExternalDepartmentId
            };

            return departmentDto;
        }

		public async Task<BankDepartmentDto> GetBankDepartmentByExternalIdAsync(int id)
		{
			var getBankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.ExternalDepartmentId == id);
			if (getBankDepartment is null)
			{
				return null;
			}
			var departmentDto = new BankDepartmentDto
			{
				Id = getBankDepartment.Id,
				BankId = getBankDepartment.BankId,
				CityId = getBankDepartment.CityId,
				DepartmentName = getBankDepartment.DepartmentName,
				DepartmentAddress = getBankDepartment.DepartmentAddress,
				ExternalDepartmentId = getBankDepartment.ExternalDepartmentId
			};

			return departmentDto;
		}

        public async Task AddExternalIdAsync(BankDepartmentDto bankDepartment)
        {
			var getBankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankdepartment => bankdepartment.Id == bankDepartment.Id);
			if (getBankDepartment != null)
			{
				getBankDepartment.ExternalDepartmentId = bankDepartment.ExternalDepartmentId;

				_bankDepartmentRepository.Update(getBankDepartment);
				await _bankDepartmentRepository.SaveChangesAsync();
			}
		}
	}
}
