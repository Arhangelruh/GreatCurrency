namespace GreatCurrency.BLL.Models
{
    public class CityDto
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
    }
}
