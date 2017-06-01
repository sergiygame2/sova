using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize(Policy = "ViewGyms")]
    [Route("Admin/Gym")]
    public class GymController : Controller
    {
        private readonly IGymRepository _gymRepo;

        public GymController(IGymRepository gymRepo)
        {
            _gymRepo = gymRepo;    
        }

        // GET: Gym
        public IActionResult Index() => View("Views/Admin/Gym/Index.cshtml",_gymRepo.GetAll());
        
        // GET: Gym/Create
        [Authorize(Policy = "CreateGyms")]
        [Route("Create")]
        public IActionResult Create()
        {
            return View("Views/Admin/Gym/Create.cshtml");
        }

        // POST: Gym/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateGyms")]
        public IActionResult Create([Bind("Id,GymName,GymRate,GymLocation,GoogleLocation,MbrshipPrice,GymArea,FoundYear,Facilities,Url,Description,GymImgUrl")] Gym gym)
        {
            if (ModelState.IsValid)
            {
                _gymRepo.Add(gym);
                return RedirectToAction("Index");
            }
            return View("Views/Admin/Gym/Create.cshtml", gym);
        }

        // GET: Gym/Edit/5
        [Route("Edit/{id}")]
        [Authorize(Policy = "UpdateGyms")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var gym = _gymRepo.Get(id);
            if (gym == null)
            {
                return NotFound();
            }
            return View("Views/Admin/Gym/Edit.cshtml",gym);
        }

        // POST: Gym/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "UpdateGyms")]
        public IActionResult Edit(int id, [Bind("Id,GymName,GymRate,GymLocation,GoogleLocation,MbrshipPrice,GymArea,FoundYear,Facilities,Url,Description,GymImgUrl")] Gym gym)
        {
            if (id != gym.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _gymRepo.Edit(gym);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymExists(gym.Id))
                        return NotFound();
                    throw;
                    
                }
                return RedirectToAction("Index");
            }
            return View("Views/Admin/Gym/Edit.cshtml", gym);
        }

        // GET: Gym/Delete/5
        [Route("Delete/{id}")]
        [Authorize(Policy = "RemoveGyms")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var gym = _gymRepo.Get(id);

            if (gym == null)
            {
                return NotFound();
            }

            return View("Views/Admin/Gym/Delete.cshtml", gym);
        }

        // POST: Gym/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RemoveGyms")]
        public IActionResult DeleteConfirmed(int id)
        {
            var gym = _gymRepo.Get(id);
            if (gym == null)
                RedirectToAction("Index");
            _gymRepo.Delete(gym);
            return RedirectToAction("Index");
        }

        private bool GymExists(int id)
        {
            return _gymRepo.GetAll().Any(gym => gym.Id == id);
        }
    }
}
