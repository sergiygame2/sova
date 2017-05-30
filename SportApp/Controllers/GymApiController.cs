using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SportApp.Models;
using SportApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SportApp.Controllers
{
    [Route("api/GymApi")]
    [Authorize(Policy = "ViewGyms")]
    public class GymApiController : ApiController<Gym>
    {
        
        public GymApiController(GymRepository repo, UserManager<IdentityUser> userManager) :
            base(repo, userManager)
        { }

        [HttpGet]
        public IActionResult Get(string _query = "", string _sort = "", string _order = "", int _start = 0, int _end = 0)
        {
            HashSet<string> searchableProperties = new HashSet<string>();
            searchableProperties.Add("Key");
            searchableProperties.Add("Value");

            return GetGeneric(_query, searchableProperties, _sort, _order, _start, _end);
        }

        [HttpGet("{id}", Name = "GetGyms")]
        public IActionResult Get(int id)
        {
            return GetByIdGeneric(id);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateGyms")]
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