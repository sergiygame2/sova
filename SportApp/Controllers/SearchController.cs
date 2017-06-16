using SportApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportApp.Models;
using System.Linq;
using System.Collections.Generic;

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
        public IActionResult Results([Bind("Region","Street", "StartPrice", "EndPrice", "Facilities" )] SearchModel searchModel)
        {
            List<string> facilities = null;
            if (!string.IsNullOrEmpty(searchModel.Facilities))
            {
                facilities = searchModel.Facilities.Split(',').ToList();
            }

            var gyms = _gymRepo.Search(searchModel.Region, searchModel.Street, searchModel.StartPrice, searchModel.EndPrice, facilities);
            ViewData["region"] = searchModel.Region;
            ViewData["street"] = searchModel.Street;
            ViewData["startprice"] = searchModel.StartPrice;
            ViewData["endprice"] = searchModel.EndPrice;
            ViewData["facilities"] = searchModel.Facilities;
            ViewData["gyms"] = JsonConvert.SerializeObject(gyms);
            return View("Results");
        }
    }
}