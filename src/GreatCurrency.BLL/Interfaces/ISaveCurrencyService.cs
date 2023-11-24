using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{

    public interface ISaveCurrencyService
    {
        /// <summary>
        /// Get currencies from site and save to database.
        /// </summary>
        /// <returns></returns>
        Task GetAndSaveAsync();

        /// <summary>
        /// Chek bank in database and save if nonfound.
        /// </summary>
        /// <param name="bankName">bank name</param>
        /// <returns>bank dto</returns>
        Task<BankDto> BankCheckOrSaveAsync(string bankName);

        /// <summary>
        /// Chek department in database and save if nonfound
        /// </summary>
        /// <param name="departmentName">department name</param>
        /// <returns>depatment dto</returns>
        Task<BankDepartmentDto> DepartmentChekOrSaveAsync(string departmentName, BankDto bank, int cityId);

    }
}
