using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SportApp.Models;
using SportApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SportApp.Services;

namespace SportApp.Controllers
{
    [Route("api/comments")]
    [Authorize(Policy = "ViewComments")]
    public class CommentApiController : ApiController<Comment>
    {
        public CommentApiController(ICommentRepository repo, IPaginationUtilities services) :
            base(repo, services)
        { }

        [HttpGet]
        public IActionResult Get(string _query = "", string _sort = "", string _order = "", int _start = 0, int _end = 0)
        {
            HashSet<string> searchableProperties = new HashSet<string> {"CommentText", "PublicationDate"};

            return GetGeneric(_query, searchableProperties, _sort, _order, _start, _end);
        }

        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult Get(int id)
        {
            return GetByIdGeneric(id);
        }

        [HttpPost]
        [Authorize(Policy = "CreateComments")]
        public override IActionResult Post([FromBody]Comment item)
        {
            return base.Post(item);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateComments")]
        public override IActionResult Put(int id, [FromBody]Comment item)
        {
            return base.Put(id, item);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RemoveComments")]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
