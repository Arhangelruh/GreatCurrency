namespace GreatCurrency.BLL.Services
{
    public class BotConfiguration
    {
        public static string Configuration = "BotConfiguration";
        /// <summary>
        /// Token.
        /// </summary>
        public string BotToken { get; set; } = default!;

        /// <summary>
        /// Host.
        /// </summary>
        public string HostAddress { get; set; } = default!;

        /// <summary>
        /// Some configurations.
        /// </summary>
        public string Route { get; set; } = default!;

        /// <summary>
        /// Secret token.
        /// </summary>
        public string SecretToken { get; set; } = default!;

        /// <summary>
        /// Chat id.
        /// </summary>
        public static string ChatId { get; set; }

    }
}
