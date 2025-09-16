using System.Text.Json.Serialization;

namespace GreatCurrency.BLL.Models.StatusBankAPIModels
{
	public class StatusBankAPILogin
	{
		/// <summary>
		/// Result status.
		/// </summary>
		[JsonPropertyName("errorInfo")]
		public ErrorInfo ErrorInfo { get; set; }

		/// <summary>
		/// Session token.
		/// </summary>
		public string? sessionToken { get; set; }
	}
}
