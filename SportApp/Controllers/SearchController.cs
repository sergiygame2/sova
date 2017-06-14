using SportApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportApp.Models;

namespace SportApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IGymRepository _gymRepo;

        public SearchController(IGymRepository repo)
        {
            _gymRepo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("SearchResults")]
        public IActionResult Results([Bind("Region","Street")] SearchModel searchModel)
        {
            var gyms = _gymRepo.Search(searchModel.Region, searchModel.Street);
            ViewData["region"] = searchModel.Region;
            ViewData["street"] = searchModel.Street;
            ViewData["gyms"] = JsonConvert.SerializeObject(gyms);
            return View("Results");
        }
    }
}