using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportApp.Models;
using SportApp.Repositories;

namespace SportApp.Controllers
{
    public class GymPageController : Controller
    {
        private readonly IGymRepository _gymRepo;
        private readonly ICommentRepository _comRepo;
        private readonly IUserGymsRepository _userRepo;



        public GymPageController(IGymRepository gymRepo, ICommentRepository comRepo, IUserGymsRepository userRepo)
        {
            _gymRepo = gymRepo;
            _comRepo = comRepo;
            _userRepo = userRepo;
        }

        [HttpGet("GymPage/{id}")]
        public IActionResult Index(int id, int Length=0)
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
        }


    }
}