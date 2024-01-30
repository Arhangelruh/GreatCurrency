namespace GreatCurrency.DAL.Models
{
    public class BankDepartment
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Department adress.
        /// </summary>
        public string DepartmentAddress { get; set; }

        /// <summary>
        /// Bank id.
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// Navigation to Bank.
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// Navigation to Bank.
        /// </summary>
        public ICollection<Currency> Currencies { get; set; }

        /// <summary>
        /// City id.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Navigation to City.
        /// </summary>
        public City City { get; set; }
    }
}
