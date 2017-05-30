using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SportApp.Repositories;
using SportApp.Models;
using SportApp.Services;


namespace SportApp.Controllers
{
    public class ApiController<T> : Controller
            where T : class, IIdentifiable
    {
        protected readonly IModelRepository<T> repo;
   
        public ApiController(IModelRepository<T> repo)
        {
            this.repo = repo;
        }

        public virtual IActionResult GetGeneric(string _query = "",
            HashSet<string> searchableProperties = null,
            string _sort = "", string _order = "", int _start = 0, int _end = 0)
        {
            var paginationService = (IPaginationUtilities)HttpContext.RequestServices.GetService(typeof(IPaginationUtilities));
            dynamic items = repo.GetAll();

            items = paginationService.Filter(items, _query, searchableProperties);
            int totalCount = items.Count;
            items = paginationService.Sort(items, _sort, _order, searchableProperties);
            items = paginationService.Partition(items, _start, _end);

            return Json(new { data = items, count = totalCount });
        }

        public IActionResult GetByIdGeneric(int id)
        {
            var entity = repo.Get(id);
            if (entity == null || entity.Id != id)
            { 
                try
                {
                    HttpContext.Response.StatusCode = 404;
                    return Json(new { message = $"{typeof(T).Name} not found" });
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return new ObjectResult(entity);
        }

        [HttpPost]
        public virtual IActionResult Post(T entity)
        {
            if (entity == null)
            {
                return BadRequest("Object reference not set to an instance of an object");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                repo.Add(entity);
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.InnerException.Message);
                ModelState.AddModelError("0", e.InnerException.Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return BadRequest($"{e.Message}");
            }
            return CreatedAtRoute(String.Concat("Get", typeof(T).Name),
                new { id = entity.Id }, entity);
        }

        [HttpPut]
        public virtual IActionResult Put(int id, [FromBody]T entity)
        {
            if (entity == null || entity.Id != id)
            {
                return BadRequest("Object reference not set to an instance of an object");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oldEntity = repo.Get(id);
            if (oldEntity == null)
            {
                try
                {
                    HttpContext.Response.StatusCode = 404;
                    return Json(new { message = $"{typeof(T).Name} not found" });
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            repo.Edit(entity);
            return new NoContentResult();
        }

        [HttpDelete]
        public virtual IActionResult Delete(int id)
        {
            var entity = repo.Get(id);
            if (entity == null)
            {
                try
                {
                    HttpContext.Response.StatusCode = 404;
                    return Json(new { message = $"{typeof(T).Name} not found" });
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            var deleteSuccessful = repo.Delete(entity);
            if (!deleteSuccessful)
            {
                return BadRequest($"Error during {typeof(T).Name} deletion");
            }
            return new NoContentResult();
        }
    }
}