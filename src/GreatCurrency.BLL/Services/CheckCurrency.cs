using GreatCurrency.BLL.Constants;
using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using System.Text;
using Telegram.Bot;

namespace GreatCurrency.BLL.Services
{
    public class CheckCurrency : ICheckCurrency
    {

        private readonly ICityService _cityService;
        private readonly IBestCurrencyService _bestCurrencyService;
        private readonly IRequestService _requestService;
        private readonly ITelegramBotClient _client;
        private readonly IBankService _bankService;


        public CheckCurrency(ICityService cityService, IBestCurrencyService bestCurrencyService, IRequestService requestService, ITelegramBotClient client, IBankService bankService)
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _bestCurrencyService = bestCurrencyService ?? throw new ArgumentNullException(nameof(bestCurrencyService));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
        }

        public async Task CheckCurrencyAsync(int mainBankId)
        {
            var cities = await _cityService.GetAllCitiesAsync();
            foreach (var city in cities)
            {
                var lastBestCourcesRequests = await _bestCurrencyService.GetLastTwoRequestsByCityAsync(city.Id);
                var lastRequest = await _requestService.GetRequestByIdAsync(lastBestCourcesRequests[0]);
                var penultimateRequest = await _requestService.GetRequestByIdAsync(lastBestCourcesRequests[1]);

                if (penultimateRequest.IncomingDate.Date != DateTime.Today)
                {
                    var ourRate = await GetOurCourseAsync(lastRequest.Id, mainBankId);
                    if (ourRate != null)
                    {
                        var message = $"В {city.CityName} установлены курсы \n" +
                            $" Покупки:\n{RatesPicturesConstant.USD} {RatesConstant.USD} {ourRate.USDBuyRate},\n {RatesPicturesConstant.EUR} {RatesConstant.EUR} {ourRate.EURBuyRate},\n {RatesPicturesConstant.RUB} {RatesConstant.RUB} {ourRate.RUBBuyRate} \n" +
                            $" Продажи:\n{RatesPicturesConstant.USD} {RatesConstant.USD} {ourRate.USDSaleRate},\n {RatesPicturesConstant.EUR} {RatesConstant.EUR} {ourRate.EURSaleRate},\n {RatesPicturesConstant.RUB} {RatesConstant.RUB} {ourRate.RUBSaleRate}";
                        await SendMessageAsync(message);

                        var betterRates = await GetBetterCoursesAsync(lastRequest.Id, mainBankId);
                        if (betterRates != null)
                        {
                            var betterRatesMessage = await CreateBetterRatesMessageAsync(betterRates);
                            await SendMessageAsync(betterRatesMessage);
                        }
                    }
                }
                else
                {
                    var messages = await CompareCurrenciesAsync(lastRequest.Id, penultimateRequest.Id, mainBankId, city.CityName);
                    if (messages.Count != 0)
                    {
                        foreach (var message in messages)
                            await SendMessageAsync(message);
                    }
                }
            }
        }

        public async Task<List<string>> CompareCurrenciesAsync(int lastRequest, int penultimateRequest, int mainBankId, string cityName)
        {
            List<string> messages = [];

            var lastCurrencies = await _bestCurrencyService.GetCurrenciesByRequestAsync(lastRequest);
            var penultimateCurrencies = await _bestCurrencyService.GetCurrenciesByRequestAsync(penultimateRequest);
            if (lastCurrencies.Count != 0 && penultimateCurrencies.Count != 0)
            {
                var ourLastRate = lastCurrencies.FirstOrDefault(rate => rate.BankId == mainBankId);
                var ourPenultimateRate = penultimateCurrencies.FirstOrDefault(rate => rate.BankId == mainBankId);
                if (ourLastRate != null && ourPenultimateRate != null)
                {
                    if (!CheckChanges(ourLastRate, ourPenultimateRate))
                    {
                        messages.Add($"В {cityName} установлены новые курсы \n:" +
                            $" Покупки:{RatesPicturesConstant.USD} {RatesConstant.USD} {ourLastRate.USDBuyRate}, {RatesPicturesConstant.EUR} {RatesConstant.EUR} {ourLastRate.EURBuyRate}, {RatesPicturesConstant.RUB} {RatesConstant.RUB} {ourLastRate.RUBBuyRate} \n" +
                            $" Продажи:{RatesPicturesConstant.USD} {RatesConstant.USD} {ourLastRate.USDSaleRate}, {RatesPicturesConstant.EUR} {RatesConstant.EUR} {ourLastRate.EURSaleRate}, {RatesPicturesConstant.RUB} {RatesConstant.RUB} {ourLastRate.RUBSaleRate}");

                        var betterRates = await GetBetterCoursesAsync(ourLastRate.RequestId, mainBankId);
                        if (betterRates != null)
                        {
                            var betterRatesMessage = await CreateBetterRatesMessageAsync(betterRates);
                            if (betterRatesMessage != "") {
								messages.Add(betterRatesMessage);
                            }                         
                        }
                        return messages;
                    }
                    else
                    {
                        foreach (var newRates in lastCurrencies)
                        {
                            var oldRate = penultimateCurrencies.FirstOrDefault(rate => rate.BankId == newRates.BankId);
                            if (oldRate != null)
                            {
                                if (!CheckChanges(newRates, oldRate))
                                {
                                    var bank = await _bankService.GetBankByIdAsync(newRates.BankId);
                                    var ourBank = await _bankService.GetBankByIdAsync(mainBankId);
                                    string firstTextPart = $"В банке {bank.BankName}, г.{cityName} изменился курс ";

                                    if (newRates.USDBuyRate != oldRate.USDBuyRate)
                                    {
                                        string secondTextPart = CheckSignal(
                                            newRates.USDBuyRate,
                                            oldRate.USDBuyRate,
                                            ourLastRate.USDBuyRate,
                                            ourPenultimateRate.USDBuyRate,
                                            RatesConstant.USD,
                                            DealMeaningConstant.Buy,
                                            bank.BankName,
                                            ourBank.BankName);
                                        if (secondTextPart != "")
                                        {
                                            messages.Add(firstTextPart + secondTextPart);
                                        }
                                    }

                                    if (newRates.USDSaleRate != oldRate.USDSaleRate)
                                    {
                                        string secondTextPart = CheckSignal(
                                            newRates.USDSaleRate,
                                            oldRate.USDSaleRate,
                                            ourLastRate.USDSaleRate,
                                            ourPenultimateRate.USDSaleRate,
                                            RatesConstant.USD,
                                            DealMeaningConstant.Sell,
                                            bank.BankName,
                                            ourBank.BankName);
                                        if (secondTextPart != "")
                                        {
                                            messages.Add(firstTextPart + secondTextPart);
                                        }
                                    }

                                    if (newRates.EURBuyRate != oldRate.EURBuyRate)
                                    {
                                        string secondTextPart = CheckSignal(
                                            newRates.EURBuyRate,
                                            oldRate.EURBuyRate,
                                            ourLastRate.EURBuyRate,
                                            ourPenultimateRate.EURBuyRate,
                                            RatesConstant.EUR,
                                            DealMeaningConstant.Buy,
                                            bank.BankName,
                                            ourBank.BankName);
                                        if (secondTextPart != "")
                                        {
                                            messages.Add(firstTextPart + secondTextPart);
                                        }
                                    }

                                    if (newRates.EURSaleRate != oldRate.EURSaleRate)
                                    {
                                        string secondTextPart = CheckSignal(
                                            newRates.EURSaleRate,
                                            oldRate.EURSaleRate,
                                            ourLastRate.EURSaleRate,
                                            ourPenultimateRate.EURSaleRate,
                                            RatesConstant.EUR,
                                            DealMeaningConstant.Sell,
                                            bank.BankName,
                                            ourBank.BankName);
                                        if (secondTextPart != "")
                                        {
                                            messages.Add(firstTextPart + secondTextPart);
                                        }
                                    }

                                    if (newRates.RUBBuyRate != oldRate.RUBBuyRate)
                                    {
                                        string secondTextPart = CheckSignal(
                                            newRates.RUBBuyRate,
                                            oldRate.RUBBuyRate,
                                            ourLastRate.RUBBuyRate,
                                            ourPenultimateRate.RUBBuyRate,
                                            RatesConstant.RUB,
                                            DealMeaningConstant.Buy,
                                            bank.BankName,
                                            ourBank.BankName);
                                        if (secondTextPart != "")
                                        {
                                            messages.Add(firstTextPart + secondTextPart);
                                        }
                                    }

                                    if (newRates.RUBSaleRate != oldRate.RUBSaleRate)
                                    {
                                        string secondTextPart = CheckSignal(
                                            newRates.RUBSaleRate,
                                            oldRate.RUBSaleRate,
                                            ourLastRate.RUBSaleRate,
                                            ourPenultimateRate.RUBSaleRate,
                                            RatesConstant.RUB,
                                            DealMeaningConstant.Sell,
                                            bank.BankName,
                                            ourBank.BankName);
                                        if (secondTextPart != "")
                                        {
                                            messages.Add(firstTextPart + secondTextPart);
                                        }
                                    }
                                }
                            }
                        }
                        return messages;
                    }
                }
            }
            return messages;
        }

        private static string CheckSignal(double newrate, double oldrate, double ournewrate, double ouroldrate, string currency, string deal, string bankName, string ourBankName)
        {

            StringBuilder stringBuilder = new StringBuilder();
            string dealMeaning = deal switch
            {
                DealMeaningConstant.Buy => "покупки",
                DealMeaningConstant.Sell => "продажи",
                _ => "",
            };

            string rate = currency switch
            {
                RatesConstant.USD => $"{RatesPicturesConstant.USD} {RatesConstant.USD}",
                RatesConstant.EUR => $"{RatesPicturesConstant.EUR} {RatesConstant.EUR}",
                RatesConstant.RUB => $"{RatesPicturesConstant.RUB} {RatesConstant.RUB}",
                _ => "",
            };

            if (deal == DealMeaningConstant.Buy)
            {
                if (oldrate < ouroldrate)
                {
                    string firstPart = $"{dealMeaning} {rate} был ниже нашего";

                    if (newrate > ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал больше нашего {bankName} {newrate} > {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate == ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал равен нашему {bankName} {newrate} = {ourBankName}{ournewrate}.");
                        return stringBuilder.ToString();
                    }
                }

                if (oldrate == ouroldrate)
                {
                    string firstPart = $"{dealMeaning} {rate} был равен нашему";
                    if (newrate > ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал больше нашего {bankName} {newrate} > {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate < ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал ниже нашего {bankName} {newrate} < {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                }

                if (oldrate > ouroldrate)
                {
                    string firstPart = $"{dealMeaning} {rate} был больше нашего";
                    if (newrate > oldrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал еще больше {bankName} {newrate} > {bankName} {oldrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate < ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал ниже нашего {bankName} {newrate} < {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate == ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал равен нашему {bankName} {newrate} = {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                }
            }

            if (deal == DealMeaningConstant.Sell)
            {
                if (oldrate < ouroldrate)
                {
                    string firstPart = $"{dealMeaning} {rate} был ниже нашего";
                    if (newrate < oldrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал еще ниже {bankName} {newrate} < {bankName} {oldrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate == ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал равен нашему {bankName} {newrate} = {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate > ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал выше нашего {bankName} {newrate} > {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                }

                if (oldrate == ouroldrate)
                {
                    string firstPart = $"{dealMeaning} {rate} был равен нашему";
                    if (newrate > ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал выше нашего {bankName} {newrate} > {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate < ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал ниже нашего {bankName} {newrate} < {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                }

                if (oldrate > ouroldrate)
                {
                    string firstPart = $"{dealMeaning} {rate} был выше нашего";
                    if (newrate < ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал ниже нашего {bankName} {newrate} < {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                    if (newrate == ournewrate)
                    {
                        stringBuilder.Append($"{firstPart} и стал равен нашему {bankName} {newrate} = {ourBankName} {ournewrate}.");
                        return stringBuilder.ToString();
                    }
                }
            }
            return stringBuilder.ToString();
        }

        private static bool CheckChanges(BestCurrencyDto lastRates, BestCurrencyDto penultimateRates)
        {

            if (lastRates.USDBuyRate != penultimateRates.USDBuyRate ||
                lastRates.USDSaleRate != penultimateRates.USDSaleRate ||
                lastRates.EURBuyRate != penultimateRates.EURBuyRate ||
                lastRates.EURSaleRate != penultimateRates.EURSaleRate ||
                lastRates.RUBBuyRate != penultimateRates.RUBBuyRate ||
                lastRates.RUBSaleRate != penultimateRates.RUBSaleRate
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<List<RateDto>?> GetBetterCoursesAsync(int lastRequest, int mainBankId)
        {

            var reqCurrencies = await _bestCurrencyService.GetCurrenciesByRequestAsync(lastRequest);

            var ourCourse = reqCurrencies.FirstOrDefault(c => c.BankId == mainBankId);
            if (ourCourse != null)
            {
                List<RateDto> bestRates = [];

                var betterUSDBuyRates = reqCurrencies.Where(rate => rate.USDBuyRate > ourCourse.USDBuyRate).ToList();
                if (betterUSDBuyRates.Count != 0)
                {
                    bestRates.GetBestRates(betterUSDBuyRates, RatesConstant.USD, DealMeaningConstant.Buy);                    
                }

                var betterUSDSellRates = reqCurrencies.Where(rate => rate.USDSaleRate < ourCourse.USDSaleRate).ToList();
                if (betterUSDSellRates.Count != 0)
                {
                    bestRates.GetBestRates(betterUSDSellRates, RatesConstant.USD, DealMeaningConstant.Sell);                    
                }

                var betterEURBuyRates = reqCurrencies.Where(rate => rate.EURBuyRate > ourCourse.EURBuyRate).ToList();
                if (betterEURBuyRates.Count != 0)
                {
                    bestRates.GetBestRates(betterEURBuyRates, RatesConstant.EUR, DealMeaningConstant.Buy);                    
                }

                var betterEURSellRates = reqCurrencies.Where(rate => rate.EURSaleRate < ourCourse.EURSaleRate).ToList();
                if (betterEURSellRates.Count != 0)
                {
                    bestRates.GetBestRates(betterEURSellRates, RatesConstant.EUR, DealMeaningConstant.Sell);                    
                }

                var betterRUBBuyRates = reqCurrencies.Where(rate => rate.RUBBuyRate > ourCourse.RUBBuyRate).ToList();
                if (betterRUBBuyRates.Count != 0)
                {
                    bestRates.GetBestRates(betterRUBBuyRates, RatesConstant.RUB, DealMeaningConstant.Buy);                    
                }

                var betterRUBSellRates = reqCurrencies.Where(rate => rate.RUBSaleRate < ourCourse.RUBSaleRate).ToList();
                if (betterRUBSellRates.Count != 0)
                {
                    bestRates.GetBestRates(betterRUBSellRates, RatesConstant.RUB, DealMeaningConstant.Sell);                    
                }

                return bestRates;
            }
            else
            {
                return null;
            }
        }

        public async Task<BestCurrencyDto?> GetOurCourseAsync(int lastRequest, int mainBankId)
        {
            var reqCurrencies = await _bestCurrencyService.GetCurrenciesByRequestAsync(lastRequest);
            var ourCourse = reqCurrencies.FirstOrDefault(c => c.BankId == mainBankId);
            if (ourCourse != null)
            {
                return ourCourse;
            }
            return null;
        }

        public async Task<string> CreateBetterRatesMessageAsync(List<RateDto> betterRates)
        {

            StringBuilder stringBuilder = new();

            string firstPart = "Есть более выгодные курсы ";

            var bestUsdBuy = betterRates.Where(rate => rate.Currency == RatesConstant.USD && rate.Meaning == DealMeaningConstant.Buy).ToList();
            var bestUsdSell = betterRates.Where(rate => rate.Currency == RatesConstant.USD && rate.Meaning == DealMeaningConstant.Sell).ToList();
            var bestEurBuy = betterRates.Where(rate => rate.Currency == RatesConstant.EUR && rate.Meaning == DealMeaningConstant.Buy).ToList();
            var bestEurSell = betterRates.Where(rate => rate.Currency == RatesConstant.EUR && rate.Meaning == DealMeaningConstant.Sell).ToList();
            var bestRubBuy = betterRates.Where(rate => rate.Currency == RatesConstant.RUB && rate.Meaning == DealMeaningConstant.Buy).ToList();
            var bestRubSell = betterRates.Where(rate => rate.Currency == RatesConstant.RUB && rate.Meaning == DealMeaningConstant.Sell).ToList();

            if (bestUsdBuy.Count != 0 || bestEurBuy.Count != 0 || bestRubBuy.Count != 0)
            {
                stringBuilder.Append(firstPart+"покупки: \n");
                if (bestUsdBuy.Count != 0)
                {
                    stringBuilder.Append($"{RatesPicturesConstant.USD} {RatesConstant.USD}\n");
                    foreach (var rate in bestUsdBuy)
                    {
                        var bank = await _bankService.GetBankByIdAsync(rate.BankId);
                        stringBuilder.Append($"{bank.BankName} {rate.Rate}\n");
                    }
                }
                if (bestEurBuy.Count != 0)
                {
                    stringBuilder.Append($"{RatesPicturesConstant.EUR} {RatesConstant.EUR}\n");
                    foreach (var rate in bestEurBuy)
                    {
                        var bank = await _bankService.GetBankByIdAsync(rate.BankId);
                        stringBuilder.Append($"{bank.BankName} {rate.Rate}\n");
                    }
                }
                if (bestRubBuy.Count != 0)
                {
                    stringBuilder.Append($"{RatesPicturesConstant.RUB} {RatesConstant.RUB}\n");
                    foreach (var rate in bestRubBuy)
                    {
                        var bank = await _bankService.GetBankByIdAsync(rate.BankId);
                        stringBuilder.Append($"{bank.BankName} {rate.Rate}\n");
                    }
                }
            }

            if (bestUsdSell.Count != 0 || bestEurSell.Count != 0 || bestRubSell.Count != 0)
            {
                stringBuilder.Append(firstPart+"продажи: \n");
                if (bestUsdSell.Count != 0)
                {
                    stringBuilder.Append($"{RatesPicturesConstant.USD} {RatesConstant.USD}\n");
                    foreach (var rate in bestUsdSell)
                    {
                        var bank = await _bankService.GetBankByIdAsync(rate.BankId);
                        stringBuilder.Append($"{bank.BankName} {rate.Rate}\n");
                    }
                }
                if (bestEurSell.Count != 0)
                {
                    stringBuilder.Append($"{RatesPicturesConstant.EUR} {RatesConstant.EUR}\n");
                    foreach (var rate in bestEurSell)
                    {
                        var bank = await _bankService.GetBankByIdAsync(rate.BankId);
                        stringBuilder.Append($"{bank.BankName} {rate.Rate}\n");
                    }
                }
                if (bestRubSell.Count != 0)
                {
                    stringBuilder.Append($"{RatesPicturesConstant.RUB} {RatesConstant.RUB}\n");
                    foreach (var rate in bestRubSell)
                    {
                        var bank = await _bankService.GetBankByIdAsync(rate.BankId);
                        stringBuilder.Append($"{bank.BankName} {rate.Rate}\n");
                    }
                }
            }
            return stringBuilder.ToString();
        }

        public async Task SendMessageAsync(string Message)
        {
            var chatId = BotConfiguration.ChatId;
            await _client.SendTextMessageAsync($"{chatId}", Message);
        }
    }
}
