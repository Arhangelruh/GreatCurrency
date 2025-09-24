using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Models;
using HtmlAgilityPack;

namespace GreatCurrency.BLL.Services
{
	public class Banki24ParserService
	{
		/// <summary>
		/// Get currency from banki24 banks table.
		/// </summary>
		/// <param name="url">main url</param>
		/// <returns>List currencies for list of organisations</returns>
		public static async Task<List<BankCurrency>> GetBanksCurrencyAsync(string url)
		{
			var currencies = new List<BankCurrency>();
			if (Banki24Constants.LegalCurrencies.Count > 0)
			{
				List<LegalRate> rates = [];

				foreach (var currency in Banki24Constants.LegalCurrencies)
				{
					HtmlWeb web = new();
					HtmlDocument doc = await web.LoadFromWebAsync(url + currency.Code);

					HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@id='courses-main']");
					if (table != null)
					{
						HtmlNode tHead = table.SelectSingleNode(".//thead");
						if (tHead != null)
						{
							HtmlNodeCollection headRows = tHead.SelectNodes(".//tr");
							HtmlNodeCollection headColumns = headRows[0].SelectNodes(".//th");
							//HtmlNode thCurrencyAmount = headRows[0].SelectSingleNode(".//th[contains(@class, 'primary')]");

							var currencyAmounthString = headColumns[1].InnerText;
							if (currencyAmounthString != null)
							{
								currencyAmounthString = currencyAmounthString.Replace(currency.Name, "");
								currencyAmounthString = currencyAmounthString.Replace(" ", "");

								if (int.TryParse(currencyAmounthString, out int currencyAmount))
								{
									HtmlNode tBody = table.SelectSingleNode(".//tbody");
									if (tBody != null)
									{
										HtmlNodeCollection rows = tBody.SelectNodes(".//tr");
										foreach (var row in rows)
										{
											HtmlNodeCollection organisationCurrency = row.SelectNodes(".//td");

											if (organisationCurrency != null)
											{
												var sellStringValue = organisationCurrency[1].InnerText.Replace(",", ".");
												var buyStringValue = organisationCurrency[2].InnerText.Replace(",", ".");

												double sellCurrencyValue = (double.TryParse(sellStringValue, out sellCurrencyValue)) ? sellCurrencyValue : 0;
												double buyCurrencyValue = (double.TryParse(buyStringValue, out buyCurrencyValue)) ? buyCurrencyValue : 0;

												var currentOrganisation = organisationCurrency[0].InnerText.Replace("&nbsp;", " ");

												var sellRate = new LegalRate
												{
													Organisation = currentOrganisation,
													Currency = currency.Name,
													Rate = sellCurrencyValue / currencyAmount,
													Meaning = "sell"
												};

												rates.Add(sellRate);

												var buyRate = new LegalRate
												{
													Organisation = currentOrganisation,
													Currency = currency.Name,
													Rate = buyCurrencyValue / currencyAmount,
													Meaning = "buy"
												};
												rates.Add(buyRate);
											}
										}
									}
								}
							}
						}
					}
				}

				if (rates.Count > 0)
				{					
					List<string> organisationList = [.. rates.Select(org => org.Organisation).Distinct()];
					foreach (var org in organisationList)
					{
						var organisationCurrency = rates.Where(o => o.Organisation == org);

						var currency = new BankCurrency
						{
							BankName = org,
							USDBuyRate = (organisationCurrency.Any(r => r.Currency == "USD")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "USD" && r.Meaning == "buy").Rate : 0,
							USDSaleRate = (organisationCurrency.Any(r => r.Currency == "USD")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "USD" && r.Meaning == "sell").Rate : 0,
							EURBuyRate = (organisationCurrency.Any(r => r.Currency == "EUR")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "EUR" && r.Meaning == "buy").Rate : 0,
							EURSaleRate = (organisationCurrency.Any(r => r.Currency == "EUR")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "EUR" && r.Meaning == "sell").Rate : 0,
							RUBBuyRate = (organisationCurrency.Any(r => r.Currency == "RUB")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "RUB" && r.Meaning == "buy").Rate : 0,
							RUBSaleRate = (organisationCurrency.Any(r => r.Currency == "RUB")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "RUB" && r.Meaning == "sell").Rate : 0,
							CNYSaleRate = (organisationCurrency.Any(r => r.Currency == "CNY")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "CNY" && r.Meaning == "sell").Rate : 0,
							CNYBuyRate = (organisationCurrency.Any(r => r.Currency == "CNY")) ? organisationCurrency.FirstOrDefault(r => r.Currency == "CNY" && r.Meaning == "buy").Rate : 0
						};

						currencies.Add(currency);
					}					
				}
			}
			return currencies;
		}

		/// <summary>
		/// Get stock currencies.
		/// </summary>
		/// <param name="url">Url for stock currency</param>
		/// <returns>Bankcurrency model with all rates</returns>
		public static async Task<BankCurrency?> GetStockCurrencyAsync(string url)
		{

			HtmlWeb web = new();
			HtmlDocument doc = await web.LoadFromWebAsync(url);

			HtmlNode exchangeContainer = doc.DocumentNode.SelectSingleNode(".//div[@class='exchange-container']");
			if (exchangeContainer != null)
			{

				HtmlNodeCollection rows = exchangeContainer.SelectNodes(".//div[@class='info-block']");
				if (rows.Count != 0)
				{
					List<LegalRate> rates = [];

					foreach (var infoBlock in rows)
					{
						HtmlNode dataContainer = infoBlock.SelectSingleNode(".//div[@class='caption']");
						if (dataContainer != null)
						{
							HtmlNodeCollection pStrings = dataContainer.SelectNodes(".//p");
							var dateBlock = pStrings[0].SelectSingleNode(".//span[@class='pull-right']");

							var currentDate = DateTime.Now.ToString("dd.MM.yyyy");

							if (dateBlock.InnerText == currentDate)
							{
								var rateDataBlock = dataContainer.SelectSingleNode(".//h2");

								var rateDataBlockNodes = rateDataBlock.ChildNodes;
								foreach (var rateNode in rateDataBlockNodes)
								{
									if (rateNode.NodeType == HtmlNodeType.Text)
									{
										if (int.TryParse(rateNode.InnerText, out int currencyAmount))
										{
											var currencyBlock = rateDataBlock.SelectSingleNode(".//a");

											foreach (var currency in Banki24Constants.LegalCurrencies)
											{
												if (currency.Name == currencyBlock.InnerText)
												{
													var stringValue = pStrings[1].InnerText.Replace(",", ".");

													if (double.TryParse(stringValue, out double currencyValue))
													{
														var rate = new LegalRate
														{
															Currency = currency.Name,
															Rate = currencyValue / currencyAmount
														};

														rates.Add(rate);
													}
												}
											}
										}
									}
								}
							}
						}
					}

					if (rates.Count > 0)
					{
						var result = new BankCurrency
						{
							USDBuyRate = (rates.Any(r => r.Currency == "USD")) ? rates.FirstOrDefault(r => r.Currency == "USD").Rate : 0,
							USDSaleRate = (rates.Any(r => r.Currency == "USD")) ? rates.FirstOrDefault(r => r.Currency == "USD").Rate : 0,
							EURBuyRate = (rates.Any(r => r.Currency == "EUR")) ? rates.FirstOrDefault(r => r.Currency == "EUR").Rate : 0,
							EURSaleRate = (rates.Any(r => r.Currency == "EUR")) ? rates.FirstOrDefault(r => r.Currency == "EUR").Rate : 0,
							RUBBuyRate = (rates.Any(r => r.Currency == "RUB")) ? rates.FirstOrDefault(r => r.Currency == "RUB").Rate : 0,
							RUBSaleRate = (rates.Any(r => r.Currency == "RUB")) ? rates.FirstOrDefault(r => r.Currency == "RUB").Rate : 0,
							CNYSaleRate = (rates.Any(r => r.Currency == "CNY")) ? rates.FirstOrDefault(r => r.Currency == "CNY").Rate : 0,
							CNYBuyRate = (rates.Any(r => r.Currency == "CNY")) ? rates.FirstOrDefault(r => r.Currency == "CNY").Rate : 0
						};

						return result;
					}
				}
			}

			return null;
		}
	}
}
