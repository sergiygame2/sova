using SportApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportApp.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportApp.Lookups;
using SportApp.Services;

namespace SportApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IGymRepository _gymRepo;

        public SearchController(IGymRepository repo)
        {
            _gymRepo = repo;
        }

        public  async Task<IActionResult> Index([Bind("Region","Street", "StartPrice", "EndPrice", "Facilities" )] SearchModel searchModel, int? page)
        {
            var defaultRegion = "Оберіть район";
            if (searchModel.Region == defaultRegion) searchModel.Region = null;
            List<string> facilities = null;
            if (!string.IsNullOrEmpty(searchModel.Facilities))
            {
                facilities = searchModel.Facilities.Split(',').ToList();
            }

            var gyms = _gymRepo.Search(searchModel.Region, searchModel.Street, searchModel.StartPrice, searchModel.EndPrice, facilities);
            var selectRegionsList = new List<string>();
            SelectLookups.Regions.ForEach(region => selectRegionsList.Add(region));
            selectRegionsList.Insert(0, defaultRegion);
            ViewData["Regions"] = new SelectList(selectRegionsList);
            ViewData["SearchModel"] = searchModel;
            ViewData["gyms"] = JsonConvert.SerializeObject(gyms.ToList());

            int pageSize = 6;
            return View(await PaginatedList<Gym>.CreateAsync(gyms, page ?? 1, pageSize));
        }
    }
}