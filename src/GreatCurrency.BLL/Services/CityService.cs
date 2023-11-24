using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
    /// <inheritdoc cref="ICityService"/>
    public class CityService : ICityService
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<BankDepartment> _bankDepartmentRepository;

        public CityService(IRepository<City> cityRepository, IRepository<BankDepartment> bankDepartmentRepository)
        {
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            _bankDepartmentRepository = bankDepartmentRepository ?? throw new ArgumentNullException(nameof(bankDepartmentRepository));
        }

        public async Task AddCityAsync(CityDto citytDto)
        {
            if (citytDto is null)
            {
                throw new ArgumentNullException(nameof(citytDto));
            }

            var newCity = new City
            {
                CityName = citytDto.CityName,
                CityURL = citytDto.CityURL
            };
            await _cityRepository.AddAsync(newCity);
            await _cityRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCityAsync(CityDto cityDto)
        {
            if (cityDto is null)
            {
                throw new ArgumentNullException(nameof(cityDto));
            }

            var bankDepartment = await _bankDepartmentRepository.GetEntityAsync(bankDepartment => bankDepartment.CityId == cityDto.Id);
            if (bankDepartment is null)
            {
                var city = await _cityRepository.GetEntityAsync(city => city.Id == cityDto.Id);
                _cityRepository.Delete(city);
                await _cityRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<CityDto>> GetAllCitiesAsync()
        {
            List<CityDto> cities = new List<CityDto>();

            var getCities = await _cityRepository.GetAll().AsNoTracking().ToListAsync();
            foreach (var city in getCities)
            {
                cities.Add(new CityDto
                {
                    Id = city.Id,
                    CityName = city.CityName,
                    CityURL = city.CityURL
                });
            }
            return cities;
        }

        public async Task<CityDto> GetCityByIdAsync(int cityid)
        {
            var getCity = await _cityRepository.GetEntityAsync(city => city.Id == cityid);
            if (getCity is null)
            {
                return null;
            }
            var cityDto = new CityDto
            {
                Id = getCity.Id,
                CityName = getCity.CityName,
                CityURL = getCity.CityURL
            };
            return cityDto;
        }

        public async Task UpdateCityAsync(CityDto cityDto)
        {
            if (cityDto is null)
            {
                throw new ArgumentNullException(nameof(cityDto));
            }
            var getCity = await _cityRepository.GetEntityAsync(city => city.Id == cityDto.Id);
            getCity.CityName = cityDto.CityName;
            getCity.CityURL = cityDto.CityURL;

            _cityRepository.Update(getCity);
            await _cityRepository.SaveChangesAsync();
        }
    }
}
