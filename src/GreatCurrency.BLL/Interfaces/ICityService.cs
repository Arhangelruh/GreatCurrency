using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
    public interface ICityService
    {
        /// <summary>
        /// Delete city.
        /// </summary>
        /// <param name="city">City dto model</param>
        Task<bool> DeleteCityAsync(CityDto cityDto);

        /// <summary>
        /// Get all cities.
        /// </summary>
        /// <returns>list Cities</returns>
        Task<List<CityDto>> GetAllCitiesAsync();

        /// <summary>
        /// Add city.
        /// </summary>
        /// <param name="cityDto">Dto model</param>
        Task AddCityAsync(CityDto cityDto);

        /// <summary>
        /// Update city.
        /// </summary>
        /// <param name="cityDto">City dto model</param>
        /// <returns></returns>
        Task UpdateCityAsync(CityDto cityDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityid">City id</param>
        /// <returns></returns>
        Task<CityDto> GetCityByIdAsync(int cityid);
    }
}
