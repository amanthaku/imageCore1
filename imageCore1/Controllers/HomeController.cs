using imageCore1.DB;
using imageCore1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Drawing.Text;

namespace imageCore1.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public readonly ApplicationEmployee Context;
        public readonly IWebHostEnvironment environment;
        public HomeController(ApplicationEmployee context, IWebHostEnvironment environment)
        {
            this.Context = context;
            this.environment = environment;
        }



        public IActionResult Read()
        {
            var data = Context.Employees.ToList();
            return View(data);
        }
        public IActionResult AddEmploye()
        {
            return View();
        }
        [HttpPost]

        public IActionResult AddEmploye(EmployTable model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    string uniqueFileName = UploadImage(model);
                    var data = new EmployTable()
                    {
                        Brand = model.Brand,
                        Description = model.Description,
                        path = uniqueFileName
                    };
                    Context.Employees.Add(data);
                    Context.SaveChanges();
                    TempData["sucess"] = "Record Sucessfully saved!";
                    return RedirectToAction("Read");
                //}
                //ModelState.AddModelError(string.Empty, "Model property os not vaild");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);


        }
        private string UploadImage(EmployTable model) {

            string uniqueFileName = string.Empty;
            if(model.ImagePath!=null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Content/emp/");
                 uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath,FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult Delete (int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = Context.Employees.Where(m=>m.Id == id).SingleOrDefault();
                if (data!=null)
                {
                    string deleteFromFolder = Path.Combine(environment.WebRootPath, "Content/emp/");
                    string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, data.path);
                    if (currentImage != null)
                    {
                        if (System.IO.File.Exists(currentImage))
                        {
                            System.IO.File.Delete(currentImage);
                        }
                    }
                    Context.Employees.Remove(data);
                    Context.SaveChanges();
                    TempData["Sucess"] = "Record Deleted!";
                }
                return RedirectToAction("Read");
            }
        }


        public IActionResult Edit(int id)
        {

            var data = Context.Employees.Where(m=>m.Id==id).SingleOrDefault();
            if (data!=null)
            {
                return View(data);
            }
            else
            {
                return RedirectToAction("Read");
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{

            //    ModelState.AddModelError(string.Empty, ex.Message);
            //}
            //return View();
        }
        [HttpPost]
        public IActionResult Edit(EmployTable model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var data = Context.Employees.Where(m=>m.Id ==  model.Id).SingleOrDefault();
                    string uniqueFileName = string.Empty;
                    if (model.ImagePath != null)
                    {
                        if (data.path != null)
                        {
                            string filepath = Path.Combine(environment.WebRootPath, "Content/emp" ,data.path);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                        uniqueFileName = UploadImage(model);
                    }
                    data.Brand = model.Brand;
                    data.Description = model.Description;
                    if (model.ImagePath != null) {
                        data.path = uniqueFileName;
                    }
                    Context.Employees.Update(data);
                    Context.SaveChanges ();
                    TempData["sucess"] = "Record Updateed Sucessfully ";
                }
                else
                {
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("Read");
        }
    }
}
