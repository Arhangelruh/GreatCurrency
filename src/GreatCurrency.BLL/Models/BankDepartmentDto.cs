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
		/// Department name.
		/// </summary>
		public required string DepartmentName { get; set; }

		/// <summary>
		/// Department adress.
		/// </summary>
		public required string DepartmentAddress { get; set; }

        /// <summary>
        /// MyFin department id.
        /// </summary>
        public int? ExternalDepartmentId { get; set; }
    }
}
