using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface IBankService
    {
        /// <summary>
        /// Delete bank.
        /// </summary>
        /// <param name="bankDto">Bank dto model</param>
        Task<bool> DeleteBankAsync(BankDto bankDto);

        /// <summary>
        /// Get all banks.
        /// </summary>
        /// <returns>list banks</returns>
        Task<List<BankDto>> GetAllBanksAsync();

        /// <summary>
        /// Add bank.
        /// </summary>
        /// <param name="bankDto">Dto model</param>
        Task<int> AddBankAsync(BankDto bankDto);

        /// <summary>
        /// Update bank.
        /// </summary>
        /// <param name="bankDto">Bank dto model</param>
        /// <returns>bank dto</returns>
        Task UpdateBankAsync(BankDto bankDto);

        /// <summary>
        /// Get bank by id.
        /// </summary>
        /// <param name="bankid">Bank id</param>
        /// <returns>bank dto</returns>
        Task<BankDto> GetBankByIdAsync(int bankid);

        /// <summary>
        /// Get bank by Name.
        /// </summary>
        /// <param name="bankName">Bank тфьу</param>
        /// <returns>bank dto</returns>
        Task<BankDto> GetBankByNameAsync(string bankName);
    }
}
