using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.DAL.Context;
using GreatCurrency.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GreatCurrency.BLL.Services
{
	/// <inheritdoc cref="ICurrencyService"/>
	public class CurrencyService(
		IRepository<Currency> currencyRepository,
		IRepository<Request> requestRepository,
		IRequestService requestService,
		IRepository<BestCurrency> bestCurrencyRepository,
		GreatCurrencyContext context) : ICurrencyService
	{
		private readonly IRepository<Currency> _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
		private readonly IRepository<Request> _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
		private readonly IRequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
		private readonly IRepository<BestCurrency> _bestCurrencyRepository = bestCurrencyRepository ?? throw new ArgumentNullException(nameof(bestCurrencyRepository));
		private readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));

		public async Task AddCurrencyAsync(CurrencyDto currencyDto)
		{
			if (currencyDto is null)
			{
				throw new ArgumentNullException(nameof(currencyDto));
			}

			var newCurrency = new Currency
			{
				BankDepartmentId = currencyDto.BankDepartmentId,
				USDBuyRate = currencyDto.USDBuyRate,
				USDSaleRate = currencyDto.USDSaleRate,
				EURBuyRate = currencyDto.EURBuyRate,
				EURSaleRate = currencyDto.EURSaleRate,
				RUBBuyRate = currencyDto.RUBBuyRate,
				RUBSaleRate = currencyDto.RUBSaleRate,
				EURUSDBuyRate = currencyDto.EURUSDBuyRate,
				EURUSDSellRate = currencyDto.EURUSDSellRate,
				USDRUBBuyRate = currencyDto.USDRUBBuyRate,
				USDRUBSellRate = currencyDto.USDRUBSellRate,
				EURRUBBuyRate = currencyDto.EURRUBBuyRate,
				EURRUBSellRate = currencyDto.EURRUBSellRate,
				RequestId = currencyDto.RequestId
			};

			await _currencyRepository.AddAsync(newCurrency);
			await _currencyRepository.SaveChangesAsync();
		}

		public async Task DeleteCurrenciesAsync(DateTime date)
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

				await _currencyRepository
					.GetAll()
					.Where(c => ids.Contains(c.RequestId))
					.ExecuteDeleteAsync();

				await _bestCurrencyRepository
					.GetAll()
					.Where(c => ids.Contains(c.RequestId))
					.ExecuteDeleteAsync();

				await _requestRepository
					.GetAll()
					.Where(r => ids.Contains(r.Id))
					.ExecuteDeleteAsync();
			}
		}

		public async Task<List<CurrencyDto>> GetCurrenciesByTimeAsync(DateTime begin, DateTime end)
		{
			var getRequests = await _requestService.GetRequestByDateBetweenAsync(begin, end);
			List<CurrencyDto> currencies = [];

			if (getRequests.Any())
			{

				foreach (var request in getRequests)
				{
					var getCurrencies = await _currencyRepository
					   .GetAll()
					   .Where(currency => currency.RequestId == request.Id)
					   .AsNoTracking()
					   .ToListAsync();

					if (getCurrencies.Any())
					{

						foreach (var currency in getCurrencies)
						{
							currencies.Add(new CurrencyDto
							{
								Id = currency.Id,
								BankDepartmentId = currency.BankDepartmentId,
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
								RequestId = currency.RequestId
							});
						}
					}
				}
			}
			return currencies;
		}
	}
}
