using GreatCurrency.BLL.Interfaces;
using GreatCurrency.Web.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
    public class HangfireController : ControllerBase
    {
        private ISaveCurrencyService _saveCurrencyService;
        private IRecurringJobManager _recurringJobManager;

        public HangfireController(ISaveCurrencyService saveCurrencyService, IRecurringJobManager recurringJobManager)
        {
            _saveCurrencyService = saveCurrencyService;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        public ActionResult CreateReccuringJob(string cron)
        {
            CronValidate Crone = new CronValidate();

            var checkCroneValue = Crone.IsValid(cron);

            if (checkCroneValue)
            {
                _recurringJobManager.AddOrUpdate("SaveCurrency", () => _saveCurrencyService.GetAndSaveAsync(), cron);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
