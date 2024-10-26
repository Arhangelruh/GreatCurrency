using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Interfaces
{
	public interface IServiceCurrencyService
	{
		/// <summary>
		/// Delete service.
		/// </summary>
		/// <param name="serviceDto">Bank dto model</param>
		Task<bool> DeleteServiceAsync(ServiceDto serviceDto);

		/// <summary>
		/// Get all services.
		/// </summary>
		/// <returns>list services</returns>
		Task<List<ServiceDto>> GetAllServicesAsync();

		/// <summary>
		/// Add service.
		/// </summary>
		/// <param name="serviceDto">Dto model</param>
		Task<int> AddServiceAsync(ServiceDto serviceDto);

		/// <summary>
		/// Update service.
		/// </summary>
		/// <param name="serviceDto">Service dto model</param>
		/// <returns>service dto</returns>
		Task UpdateServiceAsync(ServiceDto serviceDto);

		/// <summary>
		/// Get service by id.
		/// </summary>
		/// <param name="serviceid">Service id</param>
		/// <returns>service dto</returns>
		Task<ServiceDto?> GetServiceByIdAsync(int serviceid);

		/// <summary>
		/// Get service by Name.
		/// </summary>
		/// <param name="serviceName">service name</param>
		/// <returns>service dto</returns>
		Task<ServiceDto?> GetServiceByNameAsync(string serviceName);
	}
}
