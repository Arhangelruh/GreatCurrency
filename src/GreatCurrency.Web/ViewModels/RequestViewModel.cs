namespace GreatCurrency.Web.ViewModels
{
    public class RequestViewModel
    {
        /// <summary>
        /// start date.
        /// </summary>
        public DateTime? startDate { get; set; }

        /// <summary>
        /// End date.
        /// </summary>
        public DateTime? endDate { get; set; }

        /// <summary>
        /// City id.
        /// </summary>
        public int? cityId { get; set; }

        /// <summary>
        /// Bank id.
        /// </summary>
        public int? bankId { get; set; }

        /// <summary>
        /// Service id.
        /// </summary>
        public int? ServiceId { get; set; }
    }
}
