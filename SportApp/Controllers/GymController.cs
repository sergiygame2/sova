using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using GI.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SportApp.Models;
using SportApp.Repositories;
using ImageSharp;
using Microsoft.AspNetCore.Http;

namespace SportApp.Controllers
{
    [Authorize(Policy = "ViewGyms")]
    [Route("Admin/Gym")]
    public class GymController : Controller
    {
        private readonly IGymRepository _gymRepo;
        private readonly IHostingEnvironment _env;
        private readonly IOptions<UploadedFilesSettings> _filesSettings;

        public GymController(IGymRepository gymRepo, IOptions<UploadedFilesSettings> filesSettings, IHostingEnvironment env)
        {
            _gymRepo = gymRepo;
            _filesSettings = filesSettings;
            _env = env;
        }

        // GET: Gym
        public IActionResult Index() => View("Views/Admin/Gym/Index.cshtml", _gymRepo.GetAll());

        // GET: Gym/Create
        [Authorize(Policy = "CreateGyms")]
        [Route("Create")]
        [Consumes("multipart/form-data")]
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
        [Consumes("multipart/form-data")]
        public IActionResult Create([Bind("Id,GymName,GymRate,GymLocation,GoogleLocation,MbrshipPrice,GymArea,FoundYear,Facilities,Url,Description,GymImgUrl,Latitude,Longitude")] Gym gym)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files.First();
                var originalFilename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');
                string filePath = UploadImage(file, originalFilename);
                gym.GymImgUrl = !string.IsNullOrEmpty(filePath) ? filePath : "";
                _gymRepo.Add(gym);
                return RedirectToAction("Index");
            }
            return View("Views/Admin/Gym/Create.cshtml", gym);
        }

        // GET: Gym/Edit/5
        [Route("Edit/{id}")]
        [Authorize(Policy = "UpdateGyms")]
        [Consumes("multipart/form-data")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return BadRequest("Wrong id");

            var gym = _gymRepo.Get(id);
            if (gym == null)
            {
                return NotFound();
            }
            return View("Views/Admin/Gym/Edit.cshtml", gym);
        }

        // POST: Gym/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "UpdateGyms")]
        public IActionResult Edit(int id, [Bind("Id,GymName,GymRate,GymLocation,GoogleLocation,MbrshipPrice,GymArea,FoundYear,Facilities,Url,Description,GymImgUrl,Latitude,Longitude")] Gym gym)
        {
            if (id != gym.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Form.Files.First();
                    if (file != null)
                    {
                        var originalFilename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Trim('"');
                        if (!string.IsNullOrEmpty(originalFilename))
                        {
                            string filePath = UploadImage(file, originalFilename);
                            gym.GymImgUrl = !string.IsNullOrEmpty(filePath) ? filePath : "";
                        }
                    }
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

        private string UploadImage(IFormFile file, string originalFilename)
        {
            try
            {
                //var jsonpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                var jsonpath = Path.Combine(_env.WebRootPath, _filesSettings.Value.PhysicalPath.Replace("\\", "/"));
                //Directory.CreateDirectory(jsonpath);
                var path = NormalizeFilename(Path.Combine(jsonpath, originalFilename));
                const int MAX_ALLOWED_WIDTH = 300;
                string newFileName = "";
                using (Stream stream = file.OpenReadStream())
                {
                    Image image = new Image(stream);
                    if (image.Width > MAX_ALLOWED_WIDTH)
                    {
                        int neededHeight = image.Height * MAX_ALLOWED_WIDTH / image.Width;
                        newFileName = Path.GetFileNameWithoutExtension(path) + "_300" +
                                      Path.GetExtension(path);
                        string newFilePath = Path.Combine(Path.GetDirectoryName(path), newFileName);
                        using (FileStream output = System.IO.File.OpenWrite(newFilePath))
                        {
                            image.Resize(MAX_ALLOWED_WIDTH, neededHeight)
                                .Save(output);
                        }
                    }
                }
                using (FileStream fs = System.IO.File.Create(path))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var partialUrl = Path.Combine(_filesSettings.Value.RequestPath, Path.GetFileName(path)).Replace("\\", "/");
                Console.WriteLine("###" + partialUrl + "###");
                //if (newFileName != "")
                //{
                //    return Json(new { success = true, imageUrl = partialUrl, compressed = Path.Combine(filesSettings.Value.RequestPath, newFileName).Replace("\\", "/") });
                //}
                return partialUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine("###" + ex.Message + "###");
                return null;
            }
        }

        private string NormalizeFilename(string original)
        {
            var fullPath = original.Replace(" ", "_");

            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while (System.IO.File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }

    }
}
