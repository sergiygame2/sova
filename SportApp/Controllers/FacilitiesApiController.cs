using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportApp.Controllers
{
    [Produces("application/json")]
    [Route("api/facilities")]
    public class FacilitiesApiController : Controller
    {
        private IEnumerable<SelectItem> _facilities = new List<SelectItem>
        {
            new SelectItem { id = 1, text = "Trainer" },
            new SelectItem { id = 2, text = "Pool" },
            new SelectItem { id = 3, text = "Cardio zone" },
            new SelectItem { id = 4, text = "Sauna" },
            new SelectItem { id = 5, text = "Massage" },
            new SelectItem { id = 6, text = "Group workouts" },
            new SelectItem { id = 7, text = "Boxing" },
            new SelectItem { id = 8, text = "TRX" }
        };

        [HttpGet]
        public IEnumerable<SelectItem> SearchMake(string id)
        {
            var query = _facilities.Where(m => m.text.ToLower().Contains(id.ToLower()));

            return query;
        }

        [HttpGet]
        public IEnumerable<SelectItem> GetMake(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    items.Add(_facilities.FirstOrDefault(m => m.id == idInt));
                }
            }

            return items;
        }
    }

    public class SelectItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}