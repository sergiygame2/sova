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

        public GymPageController(IGymRepository gymRepo, ICommentRepository comRepo)
        {
            _gymRepo = gymRepo;
            _comRepo = comRepo;
        }

        [HttpGet("GymPage/{id}")]
        public IActionResult Index(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var gym = _gymRepo.Get(id);
            if (gym == null)
                return NotFound();

            ViewData["CurrentUserId"] = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
                return RedirectToAction($"{comment.GymId}");
            }
        }


    }
}