using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;
using SportApp.Models;
using SportApp.Repositories;

namespace SportApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _comRepo;

        public CommentController(ICommentRepository comRepo)
        {
            _comRepo = comRepo;    
        }

        // GET: Comments
        public IActionResult Index() => View(_comRepo.GetAll());
        

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,GymId,UserId,CommentText,Rate,PublicationDate")] Comment comment)
        {

            if (ModelState.IsValid)
            {
                _comRepo.Add(comment);
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var comment = _comRepo.Get(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return View(comment);
        }

        // GET: Comments/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");


            var comment = _comRepo.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var comment = _comRepo.Get(id);
            _comRepo.Delete(comment);
            return RedirectToAction("Index");
        }

        private bool CommentExists(int id)
        {
            return _comRepo.GetAll().Any(comment => comment.Id == id);
        }
    }
}
