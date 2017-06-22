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
            new SelectItem { Id = 1, Text = "Тренер" },
            new SelectItem { Id = 2, Text = "Басейн" },
            new SelectItem { Id = 3, Text = "Кардіо зона" },
            new SelectItem { Id = 4, Text = "Сауна" },
            new SelectItem { Id = 5, Text = "Масаж" },
            new SelectItem { Id = 6, Text = "Групові заняття" },
            new SelectItem { Id = 7, Text = "Бокс" },
            new SelectItem { Id = 8, Text = "TRX" }
        };

        [HttpGet("search")]
        public IEnumerable<SelectItem> SearchMake(string id)
        {
            var query = _facilities.Where(m => m.Text.ToLower().Contains(id.ToLower()));

            return query;
        }

        [HttpGet("get")]
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
                    items.Add(_facilities.FirstOrDefault(m => m.Id == idInt));
                }
            }

            return items;
        }
    }

    public class SelectItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}