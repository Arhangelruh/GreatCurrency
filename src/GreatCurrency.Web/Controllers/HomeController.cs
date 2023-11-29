using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
