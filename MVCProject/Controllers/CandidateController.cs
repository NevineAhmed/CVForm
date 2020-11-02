using CVSubTask.Models;
using CVSubTask.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace CVSubTask.Controllers
{
    public class CandidateController : Controller
    {
         CVFormTaskEntities _context=new CVFormTaskEntities();
        FileService fileService = new FileService();
        CandidateRepository Repo = new CandidateRepository();
        

        // GET: Candidate
        public ActionResult Index()
        {
            //var candidates = _context.Candidates.AsEnumerable();
            var candidates = Repo.GetAll().AsEnumerable();
            return View(candidates);

        }
        [HttpGet]
        public ActionResult SubmitCV()
        {
            return View();
        }
     
       
        
        [HttpPost]
        public ActionResult SubmitCV(Candidate model,HttpPostedFileBase cv , HttpPostedFileBase img ,string Genders, DateTime ? DateOBirth)
        {
            if (DateOBirth == null)
            {
                ModelState.AddModelError("DateError", "Enter your date of birth");
                return View();
            }

            if (!(fileService.isFileExist(cv)))
            {
                ModelState.AddModelError("FileError", "No File uploaded");
                return View();
            }

            if(!(fileService.isFileExist(img)))
            {
                ModelState.AddModelError("Image", "No Image uploaded");
                return View();

            }

            if (!(fileService.isFileSupportedFormat(cv)))
            {
                ModelState.AddModelError("FileError", "Unsupported format, only WORD and PDF are supported");
                return View();
            }

            if (!(fileService.isFileSupportedSize(cv)))                         // larger than 1024*1024
            {
                ModelState.AddModelError("FileError", "file too large, exceeded 1 MB");

                return View();
            }

           


            if (!(fileService.isImageSupported(img)))
            {
                ModelState.AddModelError("ImageError", "Not an image or exceeded 256 X 256 pixel image ");
            }


            string ImageName = Guid.NewGuid() + Path.GetExtension(img.FileName);
            string fullPath = Path.GetFullPath(ImageName);
            

            if (ModelState.IsValid)
            {
                try
                {
                    string FileName = Guid.NewGuid() + Path.GetExtension(cv.FileName);
                    cv.SaveAs(Path.Combine(Server.MapPath("~/Content/Uploads"), FileName));
                    img.SaveAs(Path.Combine(Server.MapPath("~/Content/Uploads/imgs"), ImageName));

                    var today = DateTime.Today;
                    var age = today.Year - DateOBirth.Value.Year;
                    if (age <= 0)
                    {
                        ModelState.AddModelError("DateError", "Invalid");
                        return View();
                    }

                    model.CV = FileName;
                    model.Image = ImageName;
                    model.Gender = Genders;
                    model.Age = age;

                   if(Repo.Add(model)!=null)
                    {
                        ModelState.Clear();
                        model = null;
                        ViewBag.msg = "Added!";
                    }

                    else
                    {
                        return HttpNotFound("Not found");
                    }


                    
                }
                catch (Exception e)
                {
                    ViewBag.msg = "Error Occurred ,try again";
                    return View();
                }

            }
            return View();
        }

        
        public  ActionResult Delete(int id)
        {
            // var can=_context.Candidates.FirstOrDefault(c => c.Id == id);
            var can = Repo.GetbyId(id);
            if (can != null)
            {
                //_context.Candidates.Remove(can);
                //_context.SaveChanges();
                Repo.Delete(id);
                return RedirectToAction("Index");
            }

            else
            {
                return HttpNotFound();
            }
            
        }

        public ActionResult Details(int id)
        {
            var can = Repo.GetbyId(id);
            if (can != null)
            {

                return View(can);
            }

            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Edit(int id)
        {
            var can = Repo.GetbyId(id);
            if (can != null)
            {

                return View(can);
            }

            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Edit(Candidate model, HttpPostedFileBase cv, HttpPostedFileBase img, string Genders, DateTime? DateOBirth)
        {
            if (DateOBirth == null)
            {
                ModelState.AddModelError("DateError", "Enter your date of birth");
                return View();
            }


            if (cv == null)
            {
                ModelState.AddModelError("FileError", "No File uploaded");
                return View();
            }

            if (img == null)
            {
                ModelState.AddModelError("Image", "No Image uploaded");
                return View();

            }

            if (!(fileService.isFileSupportedFormat(cv)))
            {
                ModelState.AddModelError("FileError", "Unsupported format, only WORD and PDF are supported");
                return View();
            }

            if (!(fileService.isFileSupportedSize(cv)))                         // larger than 1024*1024
            {
                ModelState.AddModelError("FileError", "file too large, exceeded 1 MB");

                return View();
            }


            if (!(fileService.isImageSupported(img)))
            {
                ModelState.AddModelError("ImageError", "Not an image or exceede 256 X 256 pixel image");
            }


            string ImageName = Guid.NewGuid() + Path.GetExtension(img.FileName);
            string fullPath = Path.GetFullPath(ImageName);


            if (ModelState.IsValid)
            {
                try
                {
                    string FileName = Guid.NewGuid() + Path.GetExtension(cv.FileName);
                    cv.SaveAs(Path.Combine(Server.MapPath("~/Content/Uploads"), FileName));
                    img.SaveAs(Path.Combine(Server.MapPath("~/Content/Uploads/imgs"), ImageName));

                    var today = DateTime.Today;
                    var age = today.Year - DateOBirth.Value.Year;
                    if (age <= 0)
                    {
                        ModelState.AddModelError("DateError", "Invalid");
                        return View();
                    }

                    model.CV = FileName;
                    model.Image = ImageName;
                    model.Gender = Genders;
                    model.Age = age;


                   if(Repo.Update(model) != null)
                    {
                        ModelState.Clear();
                        model = null;
                        ViewBag.msg = "updated!";
                    }

                    else
                    {
                        return HttpNotFound("Notfound");
                    }

                    
                }
                catch (Exception e)
                {
                    ViewBag.msg = "Error Occurred ,try again";
                    return View();
                }

            }
            return View();
        }
    }
}