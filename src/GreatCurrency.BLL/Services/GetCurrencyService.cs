using GreatCurrency.BLL.Models;
using HtmlAgilityPack;

namespace GreatCurrency.BLL.Services
{
	public class GetCurrencyService
	{
		/// <summary>
		/// Get bank office currencies.
		/// </summary>
		/// <param name="url"></param>
		/// <returns>List currencies</returns>
		public static async Task<List<BankCurrency>> GetCurrencyAsync(string url)
		{
			var currencies = new List<BankCurrency>();
			HtmlWeb web = new();
			HtmlDocument doc = await web.LoadFromWebAsync(url);

			HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@class='currencies-courses   currencies-courses--bordered']");

			if (table != null)
			{
				HtmlNodeCollection rows = table.SelectNodes(".//tr");

				if (rows != null)
				{

					foreach (HtmlNode row in rows)
					{
						if (row.Id != "")
						{
							var elements = row.Id.Split("-");
							if (elements[0] + elements[1] == "bankrow")
							{
								HtmlNodeCollection bankCells = row.SelectNodes(".//td");

								HtmlNode bankNameNode = bankCells[0].SelectSingleNode(".//span");
								string bankName = bankNameNode.InnerText;

								if (int.TryParse(elements[2], out int bankId))
								{
									HtmlNode filials = table.SelectSingleNode($".//tr[@id='filial-row-{bankId}']");

									if (filials != null)
									{
										HtmlNode courceTable = filials.SelectSingleNode(".//table");

										if (courceTable != null)
										{
											HtmlNodeCollection courseRows = courceTable.SelectNodes(".//tr");

											if (courseRows != null)
											{
												foreach (HtmlNode courseRow in courseRows)
												{
													HtmlNodeCollection cells = courseRow.SelectNodes(".//td");
													if (cells != null && cells.Count >= 6)
													{
														HtmlNode filialNameNode = cells[0].SelectSingleNode(".//a");
														string filialName = filialNameNode.InnerText;
														HtmlNode filialtimeNode = cells[0].SelectSingleNode(".//i");

														var warn = filialtimeNode.Attributes.FirstOrDefault(a => a.Value == "ic-warning mr-5");

														if (warn == null)
														{
															try
															{
																double buyUSD = double.Parse(cells[1].InnerText.Trim());
																double sellUSD = double.Parse(cells[2].InnerText.Trim());
																double buyEUR = double.Parse(cells[3].InnerText.Trim());
																double sellEUR = double.Parse(cells[4].InnerText.Trim());
																double buyRUB = double.Parse(cells[5].InnerText.Trim());
																double sellRUB = double.Parse(cells[6].InnerText.Trim());

																currencies.Add(new BankCurrency
																{
																	BankName = bankName,
																	FilialName = filialName,
																	USDBuyRate = buyUSD,
																	USDSaleRate = sellUSD,
																	EURBuyRate = buyEUR,
																	EURSaleRate = sellEUR,
																	RUBBuyRate = buyRUB,
																	RUBSaleRate = sellRUB
																});
															}
															catch
															{
																//TODO:logging
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return currencies;
		}

		/// <summary>
		/// Get currencies from services/
		/// </summary>
		/// <param name="url"></param>
		/// <returns>List currencies</returns>
		public static async Task<List<ServiceCurrency>> GetServicesCurrencyAsync(string url)
		{
			var servicesCurrencies = new List<ServiceCurrency>();

			HtmlWeb web = new();
			HtmlDocument doc = await web.LoadFromWebAsync(url);

			HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@class='currencies-courses   currencies-courses--bordered']");

			if (table != null)
			{
				HtmlNodeCollection rows = table.SelectNodes(".//tr");

				if (rows != null)
				{
					foreach (HtmlNode row in rows)
					{
						var rowType = row.GetAttributeValue("data-row-type", "");
						if (rowType == "ad")
						{
							HtmlNodeCollection serviceCells = row.SelectNodes(".//td");

							HtmlNode serviceNameNode = serviceCells[0].SelectSingleNode(".//span");
							string serviceName = serviceNameNode.InnerText;

							var checkMobile = serviceCells.Last().GetAttributeValue("class", "");
							if (checkMobile.Contains("tooltip-click"))
							{
								var serviceCurrency = new ServiceCurrency();

								foreach (var serviceCell in serviceCells)
								{
									if (serviceCell.GetAttributeValue("class", "") == "currencies-courses__currency-cell ")
									{
										try
										{
											if (!serviceCell.InnerText.Contains('-'))
											{
												var value = double.Parse(serviceCell.SelectSingleNode(".//span").InnerText);
												var rateTag = serviceCell.SelectSingleNode(".//i");
												var rate = rateTag.GetAttributeValue("data-currency", "");
												if (rateTag.GetAttributeValue("class", "").Contains("rate_buy"))
												{
													switch (rate)
													{
														case "USD":
															serviceCurrency.USDBuyRate = value;
															break;
														case "EUR":
															serviceCurrency.EURBuyRate = value;
															break;
														case "RUB":
															serviceCurrency.RUBBuyRate = value;
															break;
													}
												}

												if (rateTag.GetAttributeValue("class", "").Contains("rate_sell"))
												{
													switch (rate)
													{
														case "USD":
															serviceCurrency.USDSaleRate = value;
															break;
														case "EUR":
															serviceCurrency.EURSaleRate = value;
															break;
														case "RUB":
															serviceCurrency.RUBSaleRate = value;
															break;
													}
												}
											}
										}
										catch
										{
											//TODO: logging
										}
									}
								}
								serviceCurrency.ServiceName = serviceName;
								servicesCurrencies.Add(serviceCurrency);
							}
						}
					}
				}
			}
			return servicesCurrencies;
		}
	}
}
