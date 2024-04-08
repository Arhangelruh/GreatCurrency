using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Services;
using GreatCurrency.Web.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace GreatCurrency.Web.Controllers
{
    public class HangfireController : ControllerBase
    {
        private ISaveCurrencyService _saveCurrencyService;
        private IRecurringJobManager _recurringJobManager;
        private readonly IBankService _bankService;
        private readonly GetParameters _getParameters;

        public HangfireController(ISaveCurrencyService saveCurrencyService, IRecurringJobManager recurringJobManager, IBankService bankService, GetParameters getParameters)
        {
            _saveCurrencyService = saveCurrencyService ?? throw new ArgumentNullException(nameof(saveCurrencyService));
            _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
        }

        [HttpGet]
        public async Task<ActionResult> CreateReccuringJob(string cron)
        {
            CronValidate Crone = new CronValidate();

            var checkCroneValue = Crone.IsValid(cron);

            if (checkCroneValue)
            {
                var mainbank = await _bankService.GetBankByNameAsync(_getParameters.MainBank);
                _recurringJobManager.AddOrUpdate("SaveCurrency", () => _saveCurrencyService.GetAndSaveAsync(mainbank.Id), cron);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
