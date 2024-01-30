using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GreatCurrency.BLL.Services
{
    /// <inheritdoc cref="IBankService"/>
    public class BankService : IBankService
    {
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<BankDepartment> _bankDepartmentRepository;
        private readonly IRepository<BestCurrency> _bestCurrencyRepository;

        public BankService(IRepository<Bank> bankRepository, IRepository<BankDepartment> bankDepartmentRepository, IRepository<BestCurrency> bestCurrencyRepository)
        {
            _bankRepository = bankRepository ?? throw new ArgumentNullException(nameof(bankRepository));
            _bankDepartmentRepository = bankDepartmentRepository ?? throw new ArgumentNullException(nameof(bankDepartmentRepository));
            _bestCurrencyRepository = bestCurrencyRepository ?? throw new ArgumentNullException(nameof(bestCurrencyRepository));
        }

        public async Task<int> AddBankAsync(BankDto bankDto)
        {
            if (bankDto is null)
            {
                throw new ArgumentNullException(nameof(bankDto));
            }

            var newBank = new Bank
            {
                BankName = bankDto.BankName                
            };
            await _bankRepository.AddAsync(newBank);
            await _bankRepository.SaveChangesAsync();

            return newBank.Id;
        }

        public async Task<bool> DeleteBankAsync(BankDto bankDto)
        {
            if (bankDto is null)
            {
                throw new ArgumentNullException(nameof(bankDto));
            }
            
            var bankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankDepartment => bankDepartment.BankId == bankDto.Id);
            var bestCurrency = await _bestCurrencyRepository.GetEntityAsync(currency => currency.BankId == bankDto.Id);
            if (bankDepartment is null && bestCurrency is null)
            {
                var bank = await _bankRepository.GetEntityAsync(bank => bank.Id == bankDto.Id);
                _bankRepository.Delete(bank);
                await _bankRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<BankDto>> GetAllBanksAsync()
        {
            List<BankDto> banks = [];

            var getBanks = await _bankRepository.GetAll().AsNoTracking().ToListAsync();
            foreach (var bank in getBanks)
            {
                banks.Add(new BankDto
                {
                    Id = bank.Id,
                    BankName = bank.BankName                    
                });
            }
            return banks;
        }

        public async Task<BankDto> GetBankByIdAsync(int bankid)
        {
            var getBank = await _bankRepository.GetEntityAsync(bank => bank.Id == bankid);
            if (getBank is null)
            {
                return null;
            }
            var bankDto = new BankDto
            {
                Id = getBank.Id,
                BankName = getBank.BankName                
            };
            return bankDto;
        }

        public async Task<BankDto> GetBankByNameAsync(string bankName)
        {
            var getBank = await _bankRepository.GetEntityAsync(bank => bank.BankName == bankName);
            
            if (getBank is null)
            {
                return null;
            }
            var bankDto = new BankDto
            {
                Id = getBank.Id,
                BankName = getBank.BankName
            };
            return bankDto;
        }

        public async Task UpdateBankAsync(BankDto bankDto)
        {
            if (bankDto is null)
            {
                throw new ArgumentNullException(nameof(bankDto));
            }
            var getBank = await _bankRepository.GetEntityAsync(bank => bank.Id == bankDto.Id);
            getBank.BankName= bankDto.BankName;
            
            _bankRepository.Update(getBank);
            await _bankRepository.SaveChangesAsync();
        }
    }
}
