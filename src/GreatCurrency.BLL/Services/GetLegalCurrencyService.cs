using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.BLL.Models.StatusBankAPIModels;

namespace GreatCurrency.BLL.Services
{
	public class GetLegalCurrencyService(
		IStatusBankAPIService statusBankAPIService,
		ILEOrganisationService organisationService,
		ILERequestService requestService,
		ILECurrencyService currencyService
		) : IGetLegalCurrencyService
	{
		private readonly IStatusBankAPIService _statusbanApiService = statusBankAPIService ?? throw new ArgumentNullException(nameof(statusBankAPIService));
		private readonly ILEOrganisationService _organisationService = organisationService ?? throw new ArgumentNullException(nameof(organisationService));
		private readonly ILERequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
		private readonly ILECurrencyService _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));

		public async Task GetAndSaveAsync(string login, string password)
		{

			var statusbankAPIanswer = await _statusbanApiService.GetSessionTokenAsync(login, password);

			if (statusbankAPIanswer != null)
			{

				if (statusbankAPIanswer.sessionToken != null)
				{

					var ratesList = await _statusbanApiService.GetRatesAsync(statusbankAPIanswer.sessionToken);

					if (ratesList.ClientQuotes.Count > 0)
					{
						var requestId = await _requestService.AddRequestAsync(new LERequestDto { IncomingDate = DateTime.Now });

						var statusbankDto = await GetStatusBankCurrency(requestId, ratesList.ClientQuotes);

						await _currencyService.AddCurrencyAsync(statusbankDto);

						var stockCurrency = await Banki24ParserService.GetStockCurrencyAsync(Banki24LinksConstant.StockLink);

						if (stockCurrency != null)
						{

							var stockOrganisation = await OrganisationCheckOrSaveAsync(LegalOrganisationConstants.Stock);

							var currency = new LECurrencyDto
							{
								RequestId = requestId,
								OrganisationId = stockOrganisation,
								USDBuyRate = stockCurrency.USDBuyRate,
								USDSaleRate = stockCurrency.USDSaleRate,
								EURBuyRate = stockCurrency.EURBuyRate,
								EURSaleRate = stockCurrency.EURSaleRate,
								RUBBuyRate = stockCurrency.RUBBuyRate,
								RUBSaleRate = stockCurrency.RUBSaleRate,
								CNYBuyRate = stockCurrency.CNYBuyRate,
								CNYSaleRate = stockCurrency.CNYSaleRate
							};

							await _currencyService.AddCurrencyAsync(currency);

							
							var otherBanksRates = await Banki24ParserService.GetBanksCurrencyAsync(Banki24LinksConstant.LegalRatesLink);

							if (otherBanksRates.Count > 0)
							{
								foreach (var rate in otherBanksRates)
								{
									var organisation = await OrganisationCheckOrSaveAsync(rate.BankName);

									var otherCurrency = new LECurrencyDto
									{
										RequestId = requestId,
										OrganisationId = organisation,
										USDBuyRate = rate.USDBuyRate,
										USDSaleRate = rate.USDSaleRate,
										EURBuyRate = rate.EURBuyRate,
										EURSaleRate = rate.EURSaleRate,
										RUBBuyRate = rate.RUBBuyRate,
										RUBSaleRate = rate.RUBSaleRate,
										CNYBuyRate = rate.CNYBuyRate,
										CNYSaleRate = rate.CNYSaleRate
									};
									await _currencyService.AddCurrencyAsync(otherCurrency);
								}
							}
						}
					}
				}
			}
		}

		private async Task<int> OrganisationCheckOrSaveAsync(string name)
		{
			var getOrganisation = await _organisationService.GetOrganisationByNameAsync(name);
			if (getOrganisation != null)
			{
				return getOrganisation.Id;
			}
			else
			{
				var addOrganisation = new LEOrganisationDto { Name = name };
				var organisationId = await _organisationService.AddOrganisationAsync(addOrganisation);

				return organisationId;
			}
		}

		private async Task<LECurrencyDto> GetStatusBankCurrency(int requestId, List<ClientQuote> clientQuotes)
		{

			var statusBankId = await OrganisationCheckOrSaveAsync(LegalOrganisationConstants.StatusBank);

			var usdRates = clientQuotes.FirstOrDefault(c => c.SymbolDisplay == StatusBankConstants.USD) ?? new ClientQuote();
			var eurRates = clientQuotes.FirstOrDefault(c => c.SymbolDisplay == StatusBankConstants.EUR) ?? new ClientQuote();
			var rubRates = clientQuotes.FirstOrDefault(c => c.SymbolDisplay == StatusBankConstants.RUB) ?? new ClientQuote();
			var cnyRates = clientQuotes.FirstOrDefault(c => c.SymbolDisplay == StatusBankConstants.CNY) ?? new ClientQuote();

			var statusbankRates = new LECurrencyDto
			{
				RequestId = requestId,
				OrganisationId = statusBankId,
				USDBuyRate = usdRates.CurBid,
				USDSaleRate = usdRates.CurAsk,
				EURBuyRate = eurRates.CurBid,
				EURSaleRate = eurRates.CurAsk,
				RUBBuyRate = rubRates.CurBid,
				RUBSaleRate = rubRates.CurAsk,
				CNYBuyRate = cnyRates.CurBid,
				CNYSaleRate = cnyRates.CurAsk
			};

			return statusbankRates;
		}
	}
}
