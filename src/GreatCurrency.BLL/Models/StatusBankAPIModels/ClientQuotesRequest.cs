using System.Text.Json.Serialization;

namespace GreatCurrency.BLL.Models.StatusBankAPIModels
{
	public class ClientQuotesRequest
	{
		/// <summary>
		/// List of parameters for request.
		/// </summary>
		[JsonPropertyName("clientQuotesRequestList")]
		public List<ClientQuoteRequest> ClientQuotesRequestList { get; set; }

		/// <summary>
		/// Static data.
		/// </summary>
		public static ClientQuotesRequest Default = new ClientQuotesRequest
		{
			ClientQuotesRequestList = new List<ClientQuoteRequest>
			{
				new ClientQuoteRequest {SymbolId = 1, Currency1Volume = 50000},
				new ClientQuoteRequest {SymbolId = 4, Currency1Volume = 1000},
				new ClientQuoteRequest {SymbolId = 7, Currency1Volume = 10000},
				new ClientQuoteRequest {SymbolId = 10, Currency1Volume = 20000},
				new ClientQuoteRequest {SymbolId = 17, Currency1Volume = 10000},
				new ClientQuoteRequest {SymbolId = 18, Currency1Volume = 30000},
				new ClientQuoteRequest {SymbolId = 19, Currency1Volume = 10000},
				new ClientQuoteRequest {SymbolId = 25, Currency1Volume = 1000},
				new ClientQuoteRequest {SymbolId = 26, Currency1Volume = 1000},
				new ClientQuoteRequest {SymbolId = 27, Currency1Volume = 1000},
				new ClientQuoteRequest {SymbolId = 41, Currency1Volume = 1000}
			}
		};
	}
}
