using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="IBestCurrencyService"/>
	public class BestCurrencyService(IRepository<BestCurrency> bestCurrencyRepository, IRequestService requestService) : IBestCurrencyService
	{
		private readonly IRepository<BestCurrency> _bestCurrencyRepository = bestCurrencyRepository ?? throw new ArgumentNullException(nameof(bestCurrencyRepository));
		private readonly IRequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));

		public async Task AddCurrencyAsync(BestCurrencyDto bestCurrencyDto)
		{
			ArgumentNullException.ThrowIfNull(bestCurrencyDto);

			var newCurrency = new BestCurrency
			{
				BankId = bestCurrencyDto.BankId,
				USDBuyRate = bestCurrencyDto.USDBuyRate,
				USDSaleRate = bestCurrencyDto.USDSaleRate,
				EURBuyRate = bestCurrencyDto.EURBuyRate,
				EURSaleRate = bestCurrencyDto.EURSaleRate,
				RUBBuyRate = bestCurrencyDto.RUBBuyRate,
				RUBSaleRate = bestCurrencyDto.RUBSaleRate,
				EURUSDBuyRate = bestCurrencyDto.EURUSDBuyRate,
				EURUSDSellRate = bestCurrencyDto.EURUSDSellRate,
				USDRUBBuyRate = bestCurrencyDto.USDRUBBuyRate,
				USDRUBSellRate = bestCurrencyDto.USDRUBSellRate,
				EURRUBBuyRate = bestCurrencyDto.EURRUBBuyRate,
				EURRUBSellRate = bestCurrencyDto.EURRUBSellRate,
				RequestId = bestCurrencyDto.RequestId,
				CityId = bestCurrencyDto.CityId
			};

			await _bestCurrencyRepository.AddAsync(newCurrency);
			await _bestCurrencyRepository.SaveChangesAsync();
		}

		public async Task DeleteCurrenciesAsync(DateTime date)
		{
			var getRequests = await _requestService.GetRequestByDateAsync(date);
			if (getRequests.Count != 0)
			{
				foreach (var request in getRequests)
				{
					var getCurrencies = await _bestCurrencyRepository
					   .GetAll()
					   .Where(currency => currency.RequestId == request.Id)
					   .AsNoTracking()
					   .ToListAsync();

					if (getCurrencies.Any())
					{
						foreach (var currency in getCurrencies)
						{
							_bestCurrencyRepository.Delete(currency);
							await _bestCurrencyRepository.SaveChangesAsync();
						}
					}
					await _requestService.DeleteRequestAsync(request.Id);
				}
			}
		}

		public async Task<List<BestCurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end, int cityId)
		{
			var getRequests = await _requestService.GetRequestByDateBetweenAsync(begin, end);
			List<BestCurrencyDto> currencies = [];

			if (getRequests.Count != 0)
			{

				foreach (var request in getRequests.OrderBy(r => r.IncomingDate).ToList())
				{
					var getCurrencies = await _bestCurrencyRepository
					   .GetAll()
					   .Where(currency => currency.RequestId == request.Id && currency.CityId == cityId)
					   .AsNoTracking()
					   .ToListAsync();

					if (getCurrencies.Count != 0)
					{

						foreach (var currency in getCurrencies)
						{
							currencies.Add(new BestCurrencyDto
							{
								Id = currency.Id,
								BankId = currency.BankId,
								USDBuyRate = currency.USDBuyRate,
								USDSaleRate = currency.USDSaleRate,
								EURBuyRate = currency.EURBuyRate,
								EURSaleRate = currency.EURSaleRate,
								RUBBuyRate = currency.RUBBuyRate,
								RUBSaleRate = currency.RUBSaleRate,
								EURUSDBuyRate = currency.EURUSDBuyRate,
								EURUSDSellRate = currency.EURUSDSellRate,
								USDRUBBuyRate = currency.USDRUBBuyRate,
								USDRUBSellRate = currency.USDRUBSellRate,
								EURRUBBuyRate = currency.EURRUBBuyRate,
								EURRUBSellRate = currency.EURRUBSellRate,
								RequestId = currency.RequestId,
								RequestTime = request.IncomingDate,
								CityId = currency.CityId
							});
						}
					}
				}
			}
			return currencies;
		}

		public async Task<List<int>> GetLastTwoRequestsByCityAsync(int cityId)
		{
			List<int> requests = [];

			var getRequests = await _bestCurrencyRepository
				.GetAll()
				.Where(p => p.CityId == cityId)
				.Select(c => new { c.RequestId })
				.Distinct()
				.OrderByDescending(c => c.RequestId)
				.Take(2)
				.AsNoTracking()
				.ToListAsync();


			foreach (var request in getRequests)
			{
				requests.Add(request.RequestId);
			}

			return requests;
		}

		public async Task<List<BestCurrencyDto>> GetCurrenciesByRequestAsync(int requestId)
		{
			List<BestCurrencyDto> currencies = [];

			var getCurrencies = await _bestCurrencyRepository
				.GetAll()
				.Where(currency => currency.RequestId == requestId)
				.AsNoTracking()
				.ToListAsync();

			foreach (var currency in getCurrencies)
			{
				currencies.Add(new BestCurrencyDto
				{
					Id = currency.Id,
					BankId = currency.BankId,
					USDBuyRate = currency.USDBuyRate,
					USDSaleRate = currency.USDSaleRate,
					EURBuyRate = currency.EURBuyRate,
					EURSaleRate = currency.EURSaleRate,
					RUBBuyRate = currency.RUBBuyRate,
					RUBSaleRate = currency.RUBSaleRate,
					EURUSDBuyRate = currency.EURUSDBuyRate,
					EURUSDSellRate = currency.EURUSDSellRate,
					USDRUBBuyRate = currency.USDRUBBuyRate,
					USDRUBSellRate = currency.USDRUBSellRate,
					EURRUBBuyRate = currency.EURRUBBuyRate,
					EURRUBSellRate = currency.EURRUBSellRate,
					RequestId = currency.RequestId,
					CityId = currency.CityId
				});
			}

			return currencies;
		}

		public async Task<List<BestCurrencyDto>> GetBestCurrenciesAsync(DateTime begin, DateTime end, int cityId, int pageIndex, int pageSize)
		{
			var getCurrencies = await _bestCurrencyRepository.GetAll()
				.Where(r => r.CityId == cityId && r.Request.IncomingDate > begin && r.Request.IncomingDate < end)
				.OrderBy(r => r.RequestId)
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.AsNoTracking()
				.ToListAsync();

			List<BestCurrencyDto> currencies = [];

			if (getCurrencies.Count != 0)
			{
				foreach (var rate in getCurrencies)
				{
					var request = await _requestService.GetRequestByIdAsync(rate.RequestId);
					currencies.Add(new BestCurrencyDto
					{
						Id = rate.Id,
						BankId = rate.BankId,
						USDBuyRate = rate.USDBuyRate,
						USDSaleRate = rate.USDSaleRate,
						EURBuyRate = rate.EURBuyRate,
						EURSaleRate = rate.EURSaleRate,
						RUBBuyRate = rate.RUBBuyRate,
						RUBSaleRate = rate.RUBSaleRate,
						EURUSDBuyRate = rate.EURUSDBuyRate,
						EURUSDSellRate = rate.EURUSDSellRate,
						USDRUBBuyRate = rate.USDRUBBuyRate,
						USDRUBSellRate = rate.USDRUBSellRate,
						EURRUBBuyRate = rate.EURRUBBuyRate,
						EURRUBSellRate = rate.EURRUBSellRate,
						RequestId = rate.RequestId,
						RequestTime = request.IncomingDate,
						CityId = rate.CityId
					});

				}
			}
			return currencies;
		}

		public async Task<int> BestCurrencyCountsAsync(DateTime begin, DateTime end, int cityId)
		{
			return await _bestCurrencyRepository.GetAll().Where(r => r.CityId == cityId && r.Request.IncomingDate > begin && r.Request.IncomingDate < end).CountAsync();
		}
	}
}

