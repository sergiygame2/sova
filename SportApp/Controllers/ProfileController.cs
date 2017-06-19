using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models.DTO;
using SportApp.Repositories;
using System.Security.Claims;
using SportApp.Controllers;
using Microsoft.AspNetCore.Identity;
using SportApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SportApp.Controllers
{
    public class ProfileController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IUserGymsRepository _usersGymsRepo;
        public UserManager<ApplicationUser> _userManager;

        public ProfileController(IUserGymsRepository usersGymsRepo, UserManager<ApplicationUser> userManager)
        {
            _usersGymsRepo = usersGymsRepo;
            _userManager = userManager;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {

            string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }


            var userDTO = new UserDTO() { Id = user.Id, Email = user.Email, Password = user.PasswordHash,UserName = user.UserName,FullName = user.FullName, Address = user.Address, BirthDay= user.BirthDay, Height = user.Height, Weight = user.Weight };

            ViewData["UserGyms"] = _usersGymsRepo.GetGymsByUserId(userDTO.Id);
            return View("Views/Profile/Index.cshtml", userDTO);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(string id, [Bind("Id,Email,Password,UserName,FullName,Address,BirthDay,Height,Weight")] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Object reference not set to an instance of an object");
            }

            if (userDTO.Id != id)
            {
                return NotFound();
            }


            var userFromDb = await _userManager.FindByIdAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            userFromDb.Email = userDTO.Email;
            userFromDb.UserName = userDTO.UserName;
            userFromDb.FullName = userDTO.FullName;
            userFromDb.Address = userDTO.Address;
            userFromDb.BirthDay = userDTO.BirthDay;
            userFromDb.Height = userDTO.Height;
            userFromDb.Weight = userDTO.Weight;


            await _userManager.UpdateAsync(userFromDb);
            //if (userDTO.Roles != null)
            //{
            //    await UpdateUserRoles(userFromDb, userDTO.Roles);
            //}
            return RedirectToAction("Index");
        }


        //private bool UserDTOExists(string id)
        //{
        //    return _context.UserDTO.Any(e => e.Id == id);
        //}
    }
}
