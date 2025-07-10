using Flurl;
using Flurl.Http;
using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models.MyfinModels;

namespace GreatCurrency.BLL.Services
{
	public class MyfinAPIService : IMyfinAPIService
	{
		public async Task<MyfinAPILogin> GetBearerToken(string login, string password)
		{
			return await MyfinAPILinks.login
				.WithHeader("Content-Type", "application/x-www-form-urlencoded")
				.WithHeader("Connection", "keep-alive")
				.WithHeader("User-Agent", "statusbank")
				.PostUrlEncodedAsync(new
				{
					username = "statusbank",
					password = "XNwhvxPW"
				})
				.ReceiveJson<MyfinAPILogin>();
		}

		public async Task<List<MyfinAPIBank>> GetAllBanks(string bearerToken)
		{
			return await MyfinAPILinks.getBanks
				.WithOAuthBearerToken(bearerToken)
				.GetJsonAsync<List<MyfinAPIBank>>();
		}

		public async Task<List<MyfinAPICity>> GetAllCities(string bearerToken)
		{
			return await MyfinAPILinks.getCities
				.WithOAuthBearerToken(bearerToken)
				.GetJsonAsync<List<MyfinAPICity>>();
		}

		public async Task<List<MyfinAPICurrencyRates>> GetRateCurrenciesByCity(string bearerToken, int cityid)
		{
			int page = 1;
			int pageSize = 1000;
			var allCurrencyRates = new List<MyfinAPICurrencyRates>();

			var currencies = new[] { "USD", "EUR", "BYN" };

			while (true)
			{

				var currencyParams = new Dictionary<string, object>
				{
					["currency_codes[]"] = RatesListConstant.Rates,
					["city_id"] = cityid,
					["page"] = page,
					["per-page"] = pageSize
				};

				var response = await MyfinAPILinks.getRates
					.WithOAuthBearerToken(bearerToken)
					.SetQueryParams(currencyParams)
					.SendAsync(HttpMethod.Post);

				var responseBody = await response.GetJsonAsync<List<MyfinAPICurrencyRates>>();

				var responseHeaders = response.Headers;

				allCurrencyRates.AddRange(responseBody);

				var totalPages = responseHeaders.FirstOrDefault(h => h.Name == "X-Pagination-Page-Count").Value;

				try
				{
					if (page >= Convert.ToInt32(totalPages))
						break;

					page++;
				}
				catch
				{
					break;
				}
			}

			return allCurrencyRates;
		}
	}
}
