using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface IBankDepartmentService
    {
        /// <summary>
        /// Delete bank department.
        /// </summary>
        /// <param name="bankDepartmentDto">Bankdepartment dto model</param>
        Task<bool> DeleteBankDepartmentAsync(BankDepartmentDto bankDepartmentDto);

        /// <summary>
        /// Get all bank departments in the city.
        /// </summary>
        /// <returns>list departments</returns>
        /// <param name="cityDto">City dto model.</param>
        Task<List<BankDepartmentDto>> GetAllCityBankDepartmentsAsync(CityDto cityDto);

        /// <summary>
        /// Get all departments in bank.
        /// </summary>
        /// <returns>list departments</returns>
        /// <param name="bankDto">Bank dto model.</param>
        Task<List<BankDepartmentDto>> GetAllBankDepartmentsAsync(BankDto bankDto);

        /// <summary>
        /// Add bank department.
        /// </summary>
        /// <param name="bankDepartmentDto">Dto model</param>
        Task AddBankDepartmentAsync(BankDepartmentDto bankDepartmentDto);

        /// <summary>
        /// Get bank department by id.
        /// </summary>
        /// <param name="bankDepartmentDto">Bank department id</param>
        /// <returns></returns>
        Task<BankDepartmentDto> GetBankDepartmentByIdAsync(int bankDepartmentDto);

        /// <summary>
        /// Get bank department by name.
        /// </summary>
        /// <param name="bankDepartmentName">Bank department name</param>
        /// <returns></returns>
        Task<BankDepartmentDto> GetBankDepartmentByNameAsync(string bankDepartmentName);
    }
}
