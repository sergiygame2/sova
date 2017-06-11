using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportApp.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SportApp.Models;
using SportApp.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SportApp.Controllers
{
    public class UserDTOController : Controller
    {

        public UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        private IPaginationUtilities _paginationService;
        private IHttpContextAccessor _httpContextAccessor;

        public UserDTOController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IPaginationUtilities paginationService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _paginationService = paginationService;
            _httpContextAccessor = httpContextAccessor;
        }


        //[HttpGet]
        public async Task<IActionResult> Index(string _query = "", string _sort = "", string _order = "", int _start = 0, int _end = 0)
        {
            var allUsers = (IEnumerable<ApplicationUser>)_userManager.Users;

            HashSet<string> searchableProperties = new HashSet<string>();
            searchableProperties.Add("Email");
            searchableProperties.Add("UserName");

            allUsers = _paginationService.Filter(allUsers, _query, searchableProperties);
            allUsers = _paginationService.Sort(allUsers, _sort, _order, searchableProperties);
            var countOfUsers = allUsers.Count();
            allUsers = _paginationService.Partition(allUsers, _start, _end);

            var result = new List<UserDTO>();

            for (int i = 0; i < allUsers.Count(); i++)
            {
                var roles = (await _userManager.GetRolesAsync(allUsers.ElementAt(i))).ToDictionary(x => x, x => true);
                result.Add(new UserDTO() { Id = allUsers.ElementAt(i).Id, Email = allUsers.ElementAt(i).Email, Password = allUsers.ElementAt(i).PasswordHash, UserName = allUsers.ElementAt(i).UserName, FullName = allUsers.ElementAt(i).FullName, Address = allUsers.ElementAt(i).Address, BirthDay = allUsers.ElementAt(i).BirthDay, Height = allUsers.ElementAt(i).Height, Weight = allUsers.ElementAt(i).Weight, Roles = roles });
            }
            return View("Views/UserDTO/Index.cshtml", result);
        }

        //// GET: UserDTO
        //[HttpGet("{id}", Name = "GetUser")]
        //public async Task<IActionResult> Index(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null || user.Id != id)
        //    {
        //        return NotFound();
        //    }
        //    var roles = (await _userManager.GetRolesAsync(user)).ToDictionary(x => x, x => true);
        //    return new ObjectResult(new UserDTO() { Id = user.Id, Email = user.Email, Password = user.PasswordHash, UserName = user.UserName, FullName = user.FullName, Address = user.Address, BirthDay = user.BirthDay, Height = user.Height, Weight = user.Weight, Roles = roles });

        //}


        // GET: UserDTO/Create
        public IActionResult Create()
        {
            return View("Views/UserDTO/Create.cshtml");
        }

        // POST: UserDTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,UserName,FullName,Address,BirthDay,Height,Weight")] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                await _userManager.CreateAsync(new ApplicationUser() { Email = userDTO.Email, UserName = userDTO.UserName, FullName = userDTO.FullName, Address = userDTO.Address, BirthDay = userDTO.BirthDay, Height = userDTO.Height, Weight = userDTO.Weight, EmailConfirmed = true, LockoutEnabled = false });
                var userFromManager = await _userManager.FindByEmailAsync(userDTO.Email);
                var result = await _userManager.AddPasswordAsync(userFromManager, userDTO.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("error", "Your password should contain no less than 6 characters, at least 1 uppercase letter, 1 lowercase letter and 1 number");
                    await _userManager.DeleteAsync(userFromManager);
                    return BadRequest(ModelState);
                }
                userFromManager = await _userManager.FindByEmailAsync(userDTO.Email);
                if (userDTO.Roles != null)
                {
                    await UpdateUserRoles(userFromManager, userDTO.Roles);
                }
            }
            return RedirectToAction("Index");
        }

        //GET: UserDTO/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return BadRequest("Wrong id");


            var userDTO = await _userManager.FindByIdAsync(id);
            if (userDTO == null)
            {
                return NotFound();
            }
            var roles = (await _userManager.GetRolesAsync(userDTO)).ToDictionary(x => x, x => true);

            return View("Views/UserDTO/Edit.cshtml", new UserDTO() {Id = userDTO.Id, Email = userDTO.Email, Password = userDTO.PasswordHash, UserName = userDTO.UserName, FullName = userDTO.FullName, Address = userDTO.Address, BirthDay = userDTO.BirthDay, Height = userDTO.Height, Weight = userDTO.Weight, Roles = roles });
        }

        //// POST: UserDTO/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,Password,UserName,FullName,Address,BirthDay,Height,Weight")] UserDTO userDTO)
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
            if (userDTO.Roles != null)
            {
                await UpdateUserRoles(userFromDb, userDTO.Roles);
            }
            return RedirectToAction("Index");
        }

        // GET: UserDTO/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var userDTO = await _userManager.FindByIdAsync(id);
            if (userDTO == null)
            {
                return NotFound();
            }

            if (_userManager.Users.Count() == 1)
                return BadRequest("Last user cannot be deleted");

            return View("Views/USerDTO/Delete.cshtml", new UserDTO() { Id = userDTO.Id, Email = userDTO.Email, Password = userDTO.PasswordHash, UserName = userDTO.UserName, FullName = userDTO.FullName, Address = userDTO.Address, BirthDay = userDTO.BirthDay, Height = userDTO.Height, Weight = userDTO.Weight });
            //return View("Views/USerDTO/Delete.cshtml", user);
        }

        //// POST: UserDTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userDTO = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(userDTO);
            return RedirectToAction("Index");
        }



        private async Task UpdateUserRoles(ApplicationUser userFromDb, IDictionary<string, bool> roles)
        {
            foreach (var pair in roles)
            {
                if (await _userManager.IsInRoleAsync(userFromDb, pair.Key) && !pair.Value)
                {
                    await _userManager.RemoveFromRoleAsync(userFromDb, pair.Key);
                }
                else if (!(await _userManager.IsInRoleAsync(userFromDb, pair.Key)) && pair.Value)
                {
                    await _userManager.AddToRoleAsync(userFromDb, pair.Key);
                }
            }
        }
    }
}
