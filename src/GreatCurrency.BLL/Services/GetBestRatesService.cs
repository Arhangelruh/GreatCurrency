using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Models;

namespace GreatCurrency.BLL.Services
{
    public static class GetBestRatesService
    {
        public static void GetBestRates(this List<RateDto> list, List<BestCurrencyDto> rates, string currency, string meaning)
        {

            foreach (var bestRate in rates)
            {
                double targetRate = 0;

                if (meaning == DealMeaningConstant.Buy)
                {
                    switch (currency)
                    {
                        case RatesConstant.USD:
                            targetRate = bestRate.USDBuyRate;
                            break;
                        case RatesConstant.EUR:
                            targetRate = bestRate.EURBuyRate;
                            break;
                        case RatesConstant.RUB:
                            targetRate = bestRate.RUBBuyRate;
                            break;
                        default:
                            targetRate = 0;
                            break;
                    }
                }

                if (meaning == DealMeaningConstant.Sell)
                {
                    switch (currency)
                    {
                        case RatesConstant.USD:
                            targetRate = bestRate.USDSaleRate;
                            break;
                        case RatesConstant.EUR:
                            targetRate = bestRate.EURSaleRate;
                            break;
                        case RatesConstant.RUB:
                            targetRate = bestRate.RUBSaleRate;
                            break;
                        default:
                            targetRate = 0;
                            break;
                    }
                }

                if (targetRate != 0)
                {
                    list.Add(new RateDto
                    {
                        BankId = bestRate.BankId,
                        Currency = currency,
                        Rate = targetRate,
                        Meaning = meaning
                    });
                }
            }
        }
    }
}
