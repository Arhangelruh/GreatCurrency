using System.Text.Json.Serialization;

namespace GreatCurrency.BLL.Models.StatusBankAPIModels
{

	public class QuotesResponse
	{
		/// <summary>
		/// Result status.
		/// </summary>
		[JsonPropertyName("errorInfo")]
		public ErrorInfo ErrorInfo { get; set; }

		/// <summary>
		/// List of rates.
		/// </summary>
		[JsonPropertyName("clientQuotes")]
		public List<ClientQuote> ClientQuotes { get; set; }
	}
}
