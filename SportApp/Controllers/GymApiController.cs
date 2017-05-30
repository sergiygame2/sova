using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SportApp.Models;
using SportApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SportApp.Controllers
{
    [Route("api/gyms")]
    [Authorize(Policy = "ViewGyms")]
    public class GymApiController : ApiController<Gym>
    {
        public GymApiController(IGymRepository repo) :
            base(repo)
        { }

        [HttpGet]
        public IActionResult Get(string _query = "", string _sort = "", string _order = "", int _start = 0, int _end = 0)
        {
            HashSet<string> searchableProperties = new HashSet<string> { "GymName", "GymLocation", "Url", "FoundYear", "Description" };
            return GetGeneric(_query, searchableProperties, _sort, _order, _start, _end);
        }

        [HttpGet("{id}", Name = "GetGym")]
        public IActionResult Get(int id)
        {
            return GetByIdGeneric(id);
        }

        [HttpPost]
        [Authorize(Policy = "CreateGyms")]
        public override IActionResult Post([FromBody]Gym item)
        {
            return base.Post(item);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateGyms")]
        public override IActionResult Put(int id, [FromBody]Gym item)
        {
            return base.Put(id, item);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RemoveGyms")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

    }
}