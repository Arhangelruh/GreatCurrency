using Flurl.Http;
using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models.StatusBankAPIModels;

namespace GreatCurrency.BLL.Services
{
	public class StatusBankAPIService : IStatusBankAPIService
	{
		public async Task<StatusBankAPILogin> GetSessionTokenAsync(string login, string password)
		{
			return await StatusBankAPILinksConstant.login
				.WithHeader("Content-Type", "application/json")
				.PostJsonAsync(new { login, password })
				.ReceiveJson<StatusBankAPILogin>();
		}

		public async Task<QuotesResponse> GetRatesAsync(string token)
		{
			return await StatusBankAPILinksConstant.getRates
				.WithHeader("Content-Type", "application/json")
				.WithHeader("Session_token", token)
				.PostJsonAsync(ClientQuotesRequest.Default)
				.ReceiveJson<QuotesResponse>();
		}
	}
}
