using System.Text.Json.Serialization;

namespace GreatCurrency.BLL.Models.StatusBankAPIModels
{
	public class ClientQuoteRequest
	{
		/// <summary>
		/// Currency symbol.
		/// </summary>
		[JsonPropertyName("symbolId")]
		public int SymbolId { get; set; }

		/// <summary>
		/// Some value.
		/// </summary>
		[JsonPropertyName("currency1Volume")]
		public int Currency1Volume { get; set; }
	}
}
