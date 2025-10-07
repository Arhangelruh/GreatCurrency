using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Constants
{
	public class Banki24Constants
	{
		/// <summary>
		/// List of currencies.
		/// </summary>
		public static List<LegalCurrency> LegalCurrencies = [
			new LegalCurrency{ Name = "USD", Code = 840 },
			new LegalCurrency{ Name = "EUR", Code = 978 },
			new LegalCurrency{ Name = "RUB", Code = 643 },
			new LegalCurrency{ Name = "CNY", Code = 156 }
			];
	}
}
