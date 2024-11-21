namespace GreatCurrency.Web.Services
{
    /// <summary>
    /// Class for get parametres from appsetting.
    /// </summary>
    public class GetParameters(string mainBank, string mainService)
    {
        public string MainBank = mainBank;

        public string MainService = mainService;
    }
}
