using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="IServiceCurrencyService"/>
	public class ServiceCurrencyService(IRepository<DAL.Models.CurrencyService> serviceRepository, IRepository<CSCurrency> currencyRepository) : IServiceCurrencyService
	{
		private readonly IRepository<DAL.Models.CurrencyService> _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
		private readonly IRepository<CSCurrency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));

		public async Task<int> AddServiceAsync(ServiceDto serviceDto)
		{
			ArgumentNullException.ThrowIfNull(serviceDto, nameof(serviceDto));

			var service = new DAL.Models.CurrencyService
			{
				ServiceName = serviceDto.ServiceName,
			};
			await _serviceRepository.AddAsync(service);
			await _serviceRepository.SaveChangesAsync();

			return service.Id;
		}

		public async Task<bool> DeleteServiceAsync(ServiceDto serviceDto)
		{
			ArgumentNullException.ThrowIfNull(serviceDto);

			var currency = await _currencyRepository.GetEntityAsync(currency => currency.CurrencyServicesId == serviceDto.Id);
			if (currency is null)
			{
				var service = await _serviceRepository.GetEntityAsync(service => service.Id == serviceDto.Id);
				_serviceRepository.Delete(service);
				await _serviceRepository.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<List<ServiceDto>> GetAllServicesAsync()
		{
			List<ServiceDto> services = [];

			var getServices = await _serviceRepository.GetAll().AsNoTracking().ToListAsync();
			foreach (var service in getServices)
			{
				services.Add(new ServiceDto
				{
					Id = service.Id,
					ServiceName = service.ServiceName
				});
			}
			return services;
		}

		public async Task<ServiceDto?> GetServiceByIdAsync(int serviceid)
		{
			var getService = await _serviceRepository.GetEntityAsync(service => service.Id == serviceid);
			if (getService is null)
			{
				return null;
			}

			var serviceDto = new ServiceDto
			{
				Id = getService.Id,
				ServiceName = getService.ServiceName
			};
			return serviceDto;
		}

		public async Task<ServiceDto?> GetServiceByNameAsync(string serviceName)
		{
			var getService = await _serviceRepository.GetEntityAsync(service => service.ServiceName == serviceName);

			if (getService is null)
			{
				return null;
			}
			var serviceDto = new ServiceDto
			{
				Id = getService.Id,
				ServiceName = getService.ServiceName
			};
			return serviceDto;
		}

		public async Task UpdateServiceAsync(ServiceDto serviceDto)
		{
			ArgumentNullException.ThrowIfNull(serviceDto, nameof(serviceDto));
		
			var getService = await _serviceRepository.GetEntityAsync(service => service.Id == serviceDto.Id);
			getService.ServiceName = serviceDto.ServiceName;

			_serviceRepository.Update(getService);
			await _serviceRepository.SaveChangesAsync();
		}
	}
}
