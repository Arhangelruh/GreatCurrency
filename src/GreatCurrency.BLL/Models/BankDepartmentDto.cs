namespace GreatCurrency.BLL.Models
{
    public class BankDepartmentDto
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Bank id.
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// City id.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Department adress.
        /// </summary>
        public string DepartmentAddress { get; set; }
    }
}
