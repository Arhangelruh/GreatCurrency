using GreatCurrency.BLL.Interfaces;
using GreatCurrency.BLL.Models;
using GreatCurrency.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GreatCurrency.Web.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
        }

        /// <summary>
        /// Get all city.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Cities()
        {
            var cities = await _cityService.GetAllCitiesAsync();
            var models = new List<CityViewModel>();

            if (cities.Any())
            {

                foreach (var city in cities)
                {
                    models.Add(new CityViewModel
                    {
                        Id = city.Id,
                        CityName = city.CityName,
                        CityURL = city.CityURL
                    });
                }
            }
            return View(models);
        }

        /// <summary>
        /// Model for create city.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult CreateCity()
        {
            return View();
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="model">City view model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cities = await _cityService.GetAllCitiesAsync();
                var checkCity = cities.FirstOrDefault(city => city.CityName == model.CityName);

                if (checkCity == null)
                {
                    var city = new CityDto()
                    {
                        CityName = model.CityName,
                        CityURL = model.CityURL
                    };

                    await _cityService.AddCityAsync(city);

                    return RedirectToAction("Cities");
                }
                else
                {
                    ModelState.AddModelError("Error", "Такой город уже есть в списке.");
                    return View(model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Model for edit.
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns>View city model</returns>
        [HttpGet]
        public async Task<IActionResult> EditCity(int cityId)
        {
            var getCity = await _cityService.GetCityByIdAsync(cityId);
            if (getCity != null)
            {
                var cityViewModel = new CityViewModel
                {
                    Id = getCity.Id,
                    CityName = getCity.CityName,
                    CityURL = getCity.CityURL
                };
                return View(cityViewModel);
            }
            else
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = "Город не найден.";
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// edit city.
        /// </summary>
        /// <param name="editCity">City view model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditCity(CityViewModel editCity)
        {
            if (ModelState.IsValid)
            {
                var city = new CityDto
                {
                    Id = editCity.Id,
                    CityName = editCity.CityName,
                    CityURL = editCity.CityURL
                };

                await _cityService.UpdateCityAsync(city);
                return RedirectToAction("Cities");
            }
            else
            {
                return View(editCity.Id);
            }
        }

        /// <summary>
        /// Get city.
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns>City view model</returns>
        [HttpGet]
        public async Task<IActionResult> GetCity(int cityId)
        {
            var getcity = await _cityService.GetCityByIdAsync(cityId);
            if (getcity != null)
            {
                var city = new CityViewModel
                {
                    Id = getcity.Id,
                    CityName = getcity.CityName,
                    CityURL = getcity.CityURL
                };
                return View(city);
            }
            else
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = "Город не найден.";
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            var city = await _cityService.GetCityByIdAsync(cityId);
            if (city != null)
            {
                var deleteResult = await _cityService.DeleteCityAsync(city);
                if (deleteResult)
                {
                    return Json("success");
                }
                return Json("error");
            }
            return Json("error");
        }
    }
}
