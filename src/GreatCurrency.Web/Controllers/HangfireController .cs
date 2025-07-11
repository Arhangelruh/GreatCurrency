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
        private ISaveMyfinAPICurrencyService _saveMyfinAPICurrencyService;
        private readonly GetMyfinAPIParameters _getMyfinAPIParameters;

        public HangfireController(
            ISaveCurrencyService saveCurrencyService,
            IRecurringJobManager recurringJobManager,
            IBankService bankService,
            GetParameters getParameters,
            ISaveMyfinAPICurrencyService saveMyfinAPICurrencyService,
            GetMyfinAPIParameters getMyfinAPIParameters)
        {
            _saveCurrencyService = saveCurrencyService ?? throw new ArgumentNullException(nameof(saveCurrencyService));
            _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _getParameters = getParameters ?? throw new ArgumentNullException(nameof(getParameters));
            _saveMyfinAPICurrencyService = saveMyfinAPICurrencyService ?? throw new ArgumentNullException(nameof(saveMyfinAPICurrencyService));
            _getMyfinAPIParameters = getMyfinAPIParameters ?? throw new ArgumentNullException(nameof(getMyfinAPIParameters));

		}

        //Job с парсингом закомментирован т.к его нельзя запускать без внесение правок данных в базе или доработок кода: часть онлайн сервисов передается в апи с другим названием, так же наименованиях банков нужно чистить пробелы.
        /*
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
        */

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
	}
}
