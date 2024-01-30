using NCrontab;
namespace GreatCurrency.Web.Services
{
    public class CronValidate
    {
        public bool IsValid(string value) {        
                try
                {
                    CrontabSchedule.Parse(value.ToString());
                }
                catch
                {
                    return false;
                }
                return true;
        }
    }
}
