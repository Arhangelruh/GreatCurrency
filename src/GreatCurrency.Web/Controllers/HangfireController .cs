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
        private IRecurringJobManager _recurringJobManager;
        private readonly IBankService _bankService;
        private readonly GetParameters _getParameters;
        private ISaveMyfinAPICurrencyService _saveMyfinAPICurrencyService;
        private readonly GetMyfinAPIParameters _getMyfinAPIParameters;
        private readonly GetStatusbankParameters _getStatusbankParameters;
        private readonly IGetLegalCurrencyService _getLegalCurrencyService;

        public HangfireController(
            IRecurringJobManager recurringJobManager,
            IBankService bankService,
            GetParameters getParameters,
            ISaveMyfinAPICurrencyService saveMyfinAPICurrencyService,
            GetMyfinAPIParameters getMyfinAPIParameters,
            GetStatusbankParameters getStatusbankParameters,
            IGetLegalCurrencyService getLegalCurrencyService)
        {
            _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
            _saveMyfinAPICurrencyService = saveMyfinAPICurrencyService ?? throw new ArgumentNullException(nameof(saveMyfinAPICurrencyService));
            _getMyfinAPIParameters = getMyfinAPIParameters ?? throw new ArgumentNullException(nameof(getMyfinAPIParameters));
            _getStatusbankParameters = getStatusbankParameters ?? throw new ArgumentNullException(nameof(getStatusbankParameters));
            _getLegalCurrencyService = getLegalCurrencyService ?? throw new ArgumentNullException( nameof(getLegalCurrencyService));
		}      

		[HttpGet]
		public async Task<ActionResult> CreateReccuringJobAPI(string cron)
		{
			CronValidate Crone = new CronValidate();

			var checkCroneValue = Crone.IsValid(cron);

			if (checkCroneValue)
			{
				var mainbank = await _bankService.GetBankByNameAsync(_getParameters.MainBank);
				_recurringJobManager.AddOrUpdate("SaveCurrencyFromMyFinAPI", () => _saveMyfinAPICurrencyService.GetAndSaveAsync(mainbank.Id, _getMyfinAPIParameters.Login, _getMyfinAPIParameters.Password), cron);
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpGet]
		public ActionResult CreateReccuringJobLegalRates(string cron)
		{
			CronValidate Crone = new CronValidate();

			var checkCroneValue = Crone.IsValid(cron);

			if (checkCroneValue)
			{				
				_recurringJobManager.AddOrUpdate("SaveLegalCurrency", () => _getLegalCurrencyService.GetAndSaveAsync(_getStatusbankParameters.Login, _getStatusbankParameters.Password), cron);
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
