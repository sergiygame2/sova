using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models;
using SportApp.Repositories;

namespace SportApp.Controllers
{
    [Authorize(Policy = "ViewComments")]
    [Route("Admin/Comment")]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _comRepo;

        private readonly IGymRepository _gymRepo;

        public CommentController(ICommentRepository comRepo, IGymRepository gymRepo)
        {
            _comRepo = comRepo;
            _gymRepo = gymRepo;
        }

        // GET: Comments
        public IActionResult Index() => View("Views/Admin/Comment/Index.cshtml",_comRepo.GetAll());


        // GET: Comments/Create
        [Authorize(Policy = "CreateComments")]
        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["CurrentUserId"] = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GymsIds"] = new SelectList( _gymRepo.GetAll(), "Id", "GymName");
            return View("Views/Admin/Comment/Create.cshtml");
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateComments")]
        [Route("Create")]
        public IActionResult Create([Bind("Id,GymId,UserId,CommentText,Rate,PublicationDate")] Comment comment)
        {
            comment.PublicationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _comRepo.Add(comment);
                return RedirectToAction("Index");
            }
            var gym = _gymRepo.Get(comment.GymId);
            var comments = gym.Comments;
            int sumRate = 0;
            for (int i = 0; i < comments.Count; i++)
                sumRate += comments[i].Rate;
            double rateGym = sumRate / (comments.Count);

            ViewData["CurrentUserId"] = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GymsIds"] = new SelectList( _gymRepo.GetAll(), "Id", "GymName", comment.GymId);
            return View("Views/Admin/Comment/Create.cshtml",comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Policy = "UpdateComments")]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var comment = _comRepo.Get(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["CurrentUserId"] = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GymsIds"] = new SelectList(_gymRepo.GetAll(), "Id", "GymName", comment.GymId);
            return View("Views/Admin/Comment/Edit.cshtml",comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "UpdateComments")]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, [Bind("Id,GymId,UserId,CommentText,Rate,PublicationDate")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _comRepo.Edit(comment);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                        return NotFound();
                    throw;

                }
                return RedirectToAction("Index");
            }
            ViewData["CurrentUserId"] = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["GymsIds"] = new SelectList(_gymRepo.GetAll(), "Id", "GymName", comment.GymId);
            return View("Views/Admin/Comment/Edit.cshtml",comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Policy = "RemoveComments")]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");
            
            var comment = _comRepo.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View("Views/Admin/Comment/Delete.cshtml",comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RemoveComments")]
        [Route("Delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var comment = _comRepo.Get(id);
            if (comment == null) return BadRequest("Error during deleting comment  " + id);
            _comRepo.Delete(comment);
            return RedirectToAction("Index");
        }

        private bool CommentExists(int id)
        {
            return _comRepo.GetAll().Any(comment => comment.Id == id);
        }
    }
}
