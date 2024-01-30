using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Error page.
        /// </summary>
        /// <returns>Error view</returns>
        [Route("Error")]
        public IActionResult Error()
        {
            //var exceptionHandlerPathFeature =
            //    HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //logger.LogError($"The path {exceptionHandlerPathFeature.Path} " +
            //    $"threw an exception {exceptionHandlerPathFeature.Error}");

            return View("Error");
        }

        /// <summary>
        /// If there is 404 status code, the route path will become Error/404
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns>NotFound view</returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Запрашиваемая страница не найдена";
                    break;
                case 400:
                    ViewBag.ErrorMessage = "Ошибка запроса";
                    return BadRequest();                    
                case 0:
                    ViewBag.ErrorMessage = "Упс что-то пошло не так.";
                    break;
            }

            return View("NotFound");
        }
    }
}
