using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	public class DealStockRatesService(
		IRepository<DealStockRates> repository,
		IRepository<LERequest> requestRepository,
		ICurrenciesService currenciesService
		) : IDealStockRatesService
	{
		private readonly IRepository<DealStockRates> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
		private readonly IRepository<LERequest> _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
		private readonly ICurrenciesService _currenciesService = currenciesService ?? throw new ArgumentNullException(nameof(currenciesService));

		public async Task AddDealStokRatesAsync(List<DealStockRateDto> dealRates)
		{
			if (dealRates.Count > 0)
			{

				var currencies = await _currenciesService.GetAllCurreciesAsync();

				foreach (var dealRate in dealRates)
				{
					var currency = currencies.FirstOrDefault(c => c.Name == dealRate.Currency);
					if (currency != null)
					{
						await _repository.AddAsync(new DealStockRates
						{
							CurrencyId = currency.Id,
							Rate = dealRate.Rate,
							RequestId = dealRate.RequestId
						});
					}
					else
					{
						var currencyId = await _currenciesService.AddCurrencyAsync(new CurrenciesDto
						{
							Name = dealRate.Currency
						});

						await _repository.AddAsync(new DealStockRates
						{
							CurrencyId = currencyId,
							Rate = dealRate.Rate,
							RequestId = dealRate.RequestId
						});
					}
				}
				await _repository.SaveChangesAsync();
			}
		}

		public async Task DeleteDealStockRatesAsync(DateTime date)
		{
			const int batchSize = 5000;

			while (true)
			{
				var ids = await _requestRepository
					.GetAll()
					.Where(r => r.IncomingDate <= date)
					.Select(r => r.Id)
					.Take(batchSize)
					.ToListAsync();

				if (!ids.Any())
					break;

				await _repository
					.GetAll()
					.Where(c => ids.Contains(c.RequestId))
					.ExecuteDeleteAsync();
			}
		}

		public async Task<List<DealStockRateDto>> GetRatesByTimeAsync(DateTime begin, DateTime end)
		{
			var getrates = await _repository
				.GetAll()
				.AsNoTracking()
				.Where(r => r.LERequest.IncomingDate > begin && r.LERequest.IncomingDate < end)
				.Select(x => new DealStockRateDto
				{
					RequestId = x.RequestId,
					RequestTime = x.LERequest.IncomingDate,
					Currency = x.Currency.Name,
					Rate = x.Rate
				})
				.ToListAsync();

			return getrates;
		}
	}
}
