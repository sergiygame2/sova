using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SportApp.Repositories;
using Microsoft.AspNetCore.Identity;
using SportApp.Models;
using SportApp.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace SportApp.Controllers
{
    [Produces("application/json")]
    [Route("api/users-gyms")]
    public class UsersGymsApiController : Controller
    {
        private readonly IUserGymsRepository _usersGymsRepo;

        public UsersGymsApiController(IUserGymsRepository usersGymsRepo)
        {
            _usersGymsRepo = usersGymsRepo;
        }

        [Route("add/{gymId}")]
        [HttpPost]
        public IActionResult Add(int gymId)
        {
            string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserGyms = new UsersGyms() { GymId = gymId, ApplicationUserId = id };

            if (id == null || gymId == 0)
                return BadRequest("Wrong gymid or userid");

            try
            {
                _usersGymsRepo.Add(UserGyms);
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

            return Ok();

        }

        [Route("delete/{gymId}")]
        [HttpPost]
        public IActionResult Delete(int gymId)
        {
            string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserGyms = _usersGymsRepo.GetByUserIdAndGymId(id, gymId);

            if (id == null || gymId == 0)
                return BadRequest("Wrong gymid or userid");

            try
            {
                _usersGymsRepo.Delete(UserGyms);
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

            return Ok();
        }

    }
}