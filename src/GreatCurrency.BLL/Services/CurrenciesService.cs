using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	///<inheritdoc cref="ICurrenciesService"/>
	public class CurrenciesService(IRepository<Currencies> repository) : ICurrenciesService
	{
		private readonly IRepository<Currencies> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
		public async Task<int> AddCurrencyAsync(CurrenciesDto currency)
		{
			ArgumentNullException.ThrowIfNull(currency);

			var newCurrency = new Currencies
			{
				Name = currency.Name
			};

			await _repository.AddAsync(newCurrency);
			await _repository.SaveChangesAsync();

			return newCurrency.Id;
		}

		public async Task<List<CurrenciesDto>> GetAllCurreciesAsync()
		{
			var currencies = await _repository.GetAll().AsNoTracking().ToListAsync();
			List<CurrenciesDto> result = [];

			foreach (var currency in currencies)
			{
				result.Add(new CurrenciesDto { Id = currency.Id, Name = currency.Name });
			}
			return result;
		}

		public async Task<CurrenciesDto?> GetCurrencyByName(string currencyName)
		{
			var currency = await _repository.GetEntityWithoutTrackingAsync(c => c.Name == currencyName);
			if (currency != null)
			{
				return new CurrenciesDto { Id = currency.Id, Name = currency.Name };
			}
			else
			{
				return null;
			}
		}
	}
}
