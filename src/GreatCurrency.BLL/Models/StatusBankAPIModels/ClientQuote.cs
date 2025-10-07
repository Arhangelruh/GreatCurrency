using System.Text.Json.Serialization;

namespace GreatCurrency.BLL.Models.StatusBankAPIModels
{
	/// <summary>
	/// StatusBank API rate model.
	/// </summary>
	public class ClientQuote
	{
		/// <summary>
		/// Buy rate.
		/// </summary>
		[JsonPropertyName("cur_bid")]
		public double CurBid { get; set; }

		/// <summary>
		/// Sell rate.
		/// </summary>
		[JsonPropertyName("cur_ask")]
		public double CurAsk { get; set; }

		/// <summary>
		/// Rate symbol.
		/// </summary>
		[JsonPropertyName("symbolId")]
		public int SymbolId { get; set; }

		/// <summary>
		/// Rate pair.
		/// </summary>
		[JsonPropertyName("symbolDisplay")]
		public string SymbolDisplay { get; set; }

		/// <summary>
		/// Transaction type.
		/// </summary>
		[JsonPropertyName("transactionType")]
		public string TransactionType { get; set; }
	}
}
