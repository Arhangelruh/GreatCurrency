namespace GreatCurrency.Web.Services
{
    /// <summary>
    /// Class for get parametres from appsetting.
    /// </summary>
    public class GetParameters
    {
        public string MainBank;

        public GetParameters(string mainBank) {
            MainBank = mainBank;        
        }
    }
}
