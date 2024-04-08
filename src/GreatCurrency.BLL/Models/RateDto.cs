namespace GreatCurrency.BLL.Models
{
    public class RateDto
    {        
        /// <summary>
        /// Bank name.
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// Currency name.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Rate value.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Rate sell or buy meaning.
        /// </summary>
        public string Meaning { get; set; }
    }
}
