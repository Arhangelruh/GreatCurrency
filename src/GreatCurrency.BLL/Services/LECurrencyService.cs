using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	public class LECurrencyService(IRepository<LECurrency> currencyRepository, ILERequestService requestService) : ILECurrencyService
	{
		private readonly IRepository<LECurrency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
		private readonly ILERequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));

		public async Task AddCurrencyAsync(LECurrencyDto currencyDto)
		{
			if (currencyDto is null)
			{
				throw new ArgumentNullException(nameof(currencyDto));
			}

			var newCurrency = new LECurrency
			{
				OrganisationId = currencyDto.OrganisationId,
				USDBuyRate = currencyDto.USDBuyRate,
				USDSaleRate = currencyDto.USDSaleRate,
				EURBuyRate = currencyDto.EURBuyRate,
				EURSaleRate = currencyDto.EURSaleRate,
				RUBBuyRate = currencyDto.RUBBuyRate,
				RUBSaleRate = currencyDto.RUBSaleRate,
				CNYBuyRate = currencyDto.CNYBuyRate,
				CNYSaleRate = currencyDto.CNYSaleRate,
				RequestId = currencyDto.RequestId
			};

			await _currencyRepository.AddAsync(newCurrency);
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

		public async Task<List<LECurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end)
		{
			var getRequests = await _requestService.GetRequestByDateBetweenAsync(begin, end);
			List<LECurrencyDto> currencies = [];

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
							currencies.Add(new LECurrencyDto
							{
								Id = currency.Id,
								OrganisationId = currency.OrganisationId,
								USDBuyRate = currency.USDBuyRate,
								USDSaleRate = currency.USDSaleRate,
								EURBuyRate = currency.EURBuyRate,
								EURSaleRate = currency.EURSaleRate,
								RUBBuyRate = currency.RUBBuyRate,
								RUBSaleRate = currency.RUBSaleRate,
								CNYBuyRate = currency.CNYBuyRate,
								CNYSaleRate = currency.CNYSaleRate,
								RequestId = currency.RequestId,
								RequestTime = request.IncomingDate
							});
						}
					}
				}
			}
			return currencies;
		}
	}
}
