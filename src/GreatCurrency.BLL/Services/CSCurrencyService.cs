using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="ICSCurrencyService"/>
	public class CSCurrencyService(IRepository<CSCurrency> currencyRepository, ISCRequestService requestService) : ICSCurrencyService
	{
		private readonly IRepository<CSCurrency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
		private readonly ISCRequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));

		public async Task AddCurrencyAsync(CSCurrencyDto cSCurrencyDto)
		{
			ArgumentNullException.ThrowIfNull(cSCurrencyDto);

			var newSCCurrency = new CSCurrency
			{
				CurrencyServicesId = cSCurrencyDto.CurrencyServiceId,
				USDBuyRate = cSCurrencyDto.USDBuyRate,
				USDSaleRate = cSCurrencyDto.USDSaleRate,
				EURBuyRate = cSCurrencyDto.EURBuyRate,
				EURSaleRate = cSCurrencyDto.EURSaleRate,
				RUBBuyRate = cSCurrencyDto.RUBBuyRate,
				RUBSaleRate = cSCurrencyDto.RUBSaleRate,
				RequestId = cSCurrencyDto.RequestId
			};

			await _currencyRepository.AddAsync(newSCCurrency);
			await _currencyRepository.SaveChangesAsync();
		}

		public async Task DeleteCurrenciesAsync(DateTime date)
		{
			var getRequests = await _requestService.GetRequestByDateAsync(date);

			if (getRequests.Count != 0)
			{
				foreach (var request in getRequests)
				{
					var getCurrencies = await _currencyRepository
				   .GetAll()
				   .Where(currency => currency.RequestId == request.Id)
				   .AsNoTracking()
				   .ToListAsync();

					if (getCurrencies.Count != 0)
					{
						foreach (var currency in getCurrencies)
						{
							_currencyRepository.Delete(currency);
							await _currencyRepository.SaveChangesAsync();
						}
					}
					await _requestService.DeleteRequestAsync(request.Id);
				}
			}
		}

		public async Task<List<CSCurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end)
		{
			var getRequests = await _requestService.GetRequestByDateBetweenAsync(begin, end);
			List<CSCurrencyDto> currencies = [];

			if (getRequests.Count != 0)
			{
				foreach (var request in getRequests)
				{
					var getCurrencies = await _currencyRepository
					   .GetAll()
					   .Where(currency => currency.RequestId == request.Id)
					   .AsNoTracking()
					   .ToListAsync();

					if (getCurrencies.Count != 0)
					{
						foreach (var currency in getCurrencies)
						{
							currencies.Add(new CSCurrencyDto
							{
								Id = currency.Id,
								CurrencyServiceId = currency.CurrencyServicesId,
								USDBuyRate = currency.USDBuyRate,
								USDSaleRate = currency.USDSaleRate,
								EURBuyRate = currency.EURBuyRate,
								EURSaleRate = currency.EURSaleRate,
								RUBBuyRate = currency.RUBBuyRate,
								RUBSaleRate = currency.RUBSaleRate,
								RequestId = currency.RequestId,
								RequestTime = request.IncomingDate
							});
						}
					}
				}
			}
			return currencies;
		}

		public async Task<List<CSCurrencyDto>> GetCurrenciesWithParamsAsync(DateTime start, DateTime end, int pageIndex, int pageSize) 
		{
			var getCurrencies = await _currencyRepository.GetAll()
				   .Where(r => r.SCRequest.IncomingDate > start && r.SCRequest.IncomingDate < end)
				   .OrderBy(r => r.RequestId)
				   .Skip(pageIndex * pageSize)
				   .Take(pageSize)
				   .AsNoTracking()
				   .ToListAsync();

			List<CSCurrencyDto> currencies = [];

			if (getCurrencies.Count != 0) { 
			     foreach(var rate in getCurrencies)
				{
					var request = await _requestService.GetRequestByIdAsync(rate.RequestId);
					currencies.Add(new CSCurrencyDto
					{
						Id = rate.Id,
						CurrencyServiceId = rate.CurrencyServicesId,
						RequestId=rate.RequestId,
						RequestTime = request.IncomingDate,
						RUBBuyRate = rate.RUBBuyRate,
						RUBSaleRate = rate.RUBSaleRate,
						EURBuyRate = rate.EURBuyRate,
						EURSaleRate = rate.EURSaleRate,
						USDBuyRate = rate.USDBuyRate,
						USDSaleRate = rate.USDSaleRate
					});
				}
			}
			return currencies;
		}

		public async Task<int> CurrencyServiceCountsAsync(DateTime begin, DateTime end)
		{
			return await _currencyRepository.GetAll().Where(r => r.SCRequest.IncomingDate > begin && r.SCRequest.IncomingDate < end).CountAsync();
		}
	}
}
