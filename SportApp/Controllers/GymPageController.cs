using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportApp.Models;
using SportApp.Repositories;

namespace SportApp.Controllers
{
    public class GymPageController : Controller
    {
        private readonly IGymRepository _gymRepo;
        private readonly ICommentRepository _comRepo;
        private readonly IUserGymsRepository _userRepo;

        public UserManager<ApplicationUser> _userManager;


        public GymPageController(UserManager<ApplicationUser> userManager, IGymRepository gymRepo, ICommentRepository comRepo, IUserGymsRepository userRepo)
        {
            _gymRepo = gymRepo;
            _comRepo = comRepo;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        [HttpGet("GymPage/{id}")]
        public async Task<IActionResult> Index(int id, int Length=0)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var gym = _gymRepo.Get(id);
            if (gym == null)
                return NotFound();

            if(Length!=0) ViewData["Errors"] = "error";
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            ViewData["CurrentUserId"] = userId;

            //userId gym.id
            if(_userRepo.GetByUserIdAndGymId(userId, gym.Id)==null)
                ViewData["MyGym"] = false; 
            else
                ViewData["MyGym"] = true;
            
            if(userId == null) userId = "";
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewData["LoggedIn"] = true;
                ViewData["CurrentUserName"] = user.FullName;
                ViewData["CurrentUserLogin"] = user.UserName;
            }
            else
            {
                ViewData["LoggedIn"] = false;
                ViewData["CurrentUserName"] = ""; 
                ViewData["CurrentUserLogin"] = "";
            }

            return View(gym);
        }

        [HttpPost]
        public IActionResult CreateComment([Bind("Id,GymId,UserId,CommentText,Rate,PublicationDate")] Comment comment)
        {
            comment.PublicationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _comRepo.Add(comment);
                return RedirectToAction($"{comment.GymId}");
            }
            else if (comment.UserId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction($"{comment.GymId}","GymPage","error");
            }


            var gym = _gymRepo.Get(comment.GymId);
            var comments = gym.Comments;
            int sumRate = 0;
            for (int i = 0; i < comments.Count; i++)
                sumRate += comments[i].Rate;
            double rateGym = sumRate / comments.Count;
            int rate = (int)Math.Round(rateGym);
            gym.GymRate = rate;
            try
            {
                _gymRepo.Edit(gym);
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


        }


    }
}