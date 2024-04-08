using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace GreatCurrency.BLL.Services
{
    /// <summary>
    /// TelegramBot extension method.
    /// </summary>
    public static class TelegramBotExtensions
    {
        /// <summary>
        /// Add TelegramBot client.
        /// </summary>
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection, IConfigurationSection configuration)
        {
            var token = configuration.GetSection("BotToken");
            var client = new TelegramBotClient(token.Value);
            var webHook = $"{configuration.GetSection("HostAddress").Value}{configuration.GetSection("Route").Value}/api/message/update";
            client.SetWebhookAsync(webHook).Wait();

            serviceCollection.AddTransient<ITelegramBotClient>(x => client);

            return serviceCollection;
        }
    }
}
