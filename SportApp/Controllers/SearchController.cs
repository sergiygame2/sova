using SportApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        
        public IActionResult Results()
        {
            ViewData["gyms"] = JsonConvert.SerializeObject(_gymRepo.GetAll());
            return View("Results");
        }
    }
}