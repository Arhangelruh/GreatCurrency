using System.Text.Json.Serialization;

namespace GreatCurrency.BLL.Models.StatusBankAPIModels
{
	/// <summary>
	/// Result of request.
	/// </summary>
	public class ErrorInfo
	{

		/// <summary>
		/// Result status.
		/// </summary>
		[JsonPropertyName("error")]
		public string Error { get; set; }

		/// <summary>
		/// Result description.
		/// </summary>
		[JsonPropertyName("errorDescription")]
		public string ErrorDescription { get; set; }
	}
}
