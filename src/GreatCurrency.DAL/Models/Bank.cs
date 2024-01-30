namespace GreatCurrency.DAL.Models
{
    public class Bank 
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Bank Name.
        /// </summary>
        public string BankName { get; set; }

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
