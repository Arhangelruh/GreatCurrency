namespace GreatCurrency.DAL.Models
{
    public class City
    {
        /// <summary>
        /// City id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// City name.
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Url for request.
        /// </summary>
        public string? CityURL { get; set; }

        /// <summary>
        /// Navigation to Bank Departments.
        /// </summary>
        public ICollection<BankDepartment> BankDepartments { get; set; }

        /// <summary>
        /// Navigation to Best Currency.
        /// </summary>
        public ICollection<BestCurrency> BestCurrencies { get; set; }
    }
}
