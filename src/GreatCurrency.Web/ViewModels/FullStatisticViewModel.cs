namespace GreatCurrency.Web.ViewModels
{
    public class FullStatisticViewModel
    {
        /// <summary>
        /// Bank Statistic view model.
        /// </summary>
        public StatisticViewModel Statistic { get; set; }

        /// <summary>
        /// Request view model.
        /// </summary>
        public RequestViewModel Request { get; set; }

        /// <summary>
        /// Service statistic view model.
        /// </summary>
        public ServiceStatisticViewModel ServiceStatistic { get; set; }
    }
}
