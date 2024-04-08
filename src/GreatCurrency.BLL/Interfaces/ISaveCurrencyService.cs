using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    /// <summary>
    /// Parse currencies from myfin and save to base.
    /// </summary>
    public interface ISaveCurrencyService
    {
        /// <summary>
        /// Get currencies from site and save to database.
        /// </summary>
        /// <returns></returns>
        Task GetAndSaveAsync(int mainBankId);

        /// <summary>
        /// Chek bank in database and save if nonfound.
        /// </summary>
        /// <param name="bankName">bank name</param>
        /// <returns>bank dto</returns>
        Task<int> BankCheckOrSaveAsync(string bankName);

        /// <summary>
        /// Chek department in database and save if nonfound
        /// </summary>
        /// <param name="departmentName">department name</param>
        /// <returns>depatment dto</returns>
        Task<int> DepartmentChekOrSaveAsync(string departmentName, int bankId, int cityId);

    }
}
