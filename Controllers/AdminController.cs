using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.EF;

namespace Project.Controllers
{
    public class AdminController : Controller
    {
        readonly Project_databaseEntities db = new Project_databaseEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Driver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_Driver(Driver driver, HttpPostedFileBase photo)
        {
            if(ModelState.IsValid)
            {
                string _FileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string _path = Path.Combine(Server.MapPath("~/uploads"), _FileName);
                photo.SaveAs(_path);
                driver.Photo = _FileName;
                driver.Validity = DateTime.Now.AddYears(2).ToString();
                driver.Is_wanted = "false";
                db.Drivers.Add(driver);
                db.SaveChanges();
                return RedirectToAction("Driver_List");
            }
            return View(driver);
        }

        [HttpGet]
        public ActionResult Driver_List(int id = 0)
        {
            //var param = this.Request.Params["skip"];
            var d = db.Drivers.ToList().Skip(id * 10).Take(10);
            return View(d);
        }

        [HttpGet]
        public ActionResult Update_Driver(int id)
        {
            var driver = (from d in db.Drivers where d.Id==id select d).SingleOrDefault();
            return View(driver);
        }

        [HttpPost]
        public ActionResult Update_Driver(Driver driver, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                Driver dr = (from d in db.Drivers where d.Id == driver.Id select d).SingleOrDefault();
                driver.Date_of_issue = dr.Date_of_issue;
                dr.First_name = driver.First_name;
                dr.Last_name = driver.Last_name;
                dr.Email = driver.Email;
                dr.Phone = driver.Phone;
                dr.Gender = driver.Gender;
                dr.Date_of_birth = driver.Date_of_birth;
                dr.Age = driver.Age;
                dr.Blood_group = driver.Blood_group;
                dr.Father_name = driver.Father_name;
                dr.Address = driver.Address;
                dr.Driving_license_number = driver.Driving_license_number;
                dr.Category = driver.Category;
                if(driver.Validity!=null)
                {
                    dr.Validity = DateTime.Now.AddYears(2).ToString();
                }
                dr.Issuing_authority = driver.Issuing_authority;
                if(driver.Is_wanted==null)
                {
                    dr.Is_wanted = "false";
                }
                else
                {
                    dr.Is_wanted = "true";
                }
                if(photo!=null)
                {
                    string oldName = dr.Photo.Split('.')[0];
                    string _FileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    string _path = Path.Combine(Server.MapPath("~/uploads"), _FileName);
                    photo.SaveAs(_path);
                    string[] files = Directory.GetFileSystemEntries(Server.MapPath("~/uploads"), $"{oldName}.*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }
                    dr.Photo = _FileName;
                }
                try
                {
                    _ = db.SaveChanges();
                    return RedirectToAction("Driver_List");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return View(driver);
        }

        [HttpGet]
        public ActionResult Add_Surgeon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_Surgeon(Surgeon sg)
        {
            if(ModelState.IsValid)
            {
                db.Surgeons.Add(sg);
                db.SaveChanges();
                return RedirectToAction("Surgeon_List");
            }
            return View(sg);
        }

        [HttpGet]
        public ActionResult Surgeon_List(int id=0)
        {
            //var param = this.Request.Params["skip"];
            var s = db.Surgeons.ToList().Skip(id*0).Take(10);
            return View(s);
        }

        [HttpGet]
        public ActionResult Update_Surgeon(int id)
        {
            //var param = this.Request.Params["skip"];
            var s = db.Surgeons.Find(id);
            return View(s);
        }

        [HttpPost]
        public ActionResult Update_Surgeon(Surgeon surgeon,int id)
        {
            //var param = this.Request.Params["skip"];
            if(ModelState.IsValid)
            {
                var data = (from s in db.Surgeons where s.Id == id select s).SingleOrDefault();
                data.Password = surgeon.Password;
                data.Zone = surgeon.Zone;
                db.SaveChanges();
                return RedirectToAction("Surgeon_List");
            }
            return View(surgeon);
        }

        [HttpGet]
        public ActionResult Rules()
        {
            return View(db.Offences.ToList());
        }

        [HttpPost]
        public ActionResult Rules(Offence offence)
        {
            if(ModelState.IsValid)
            {
                db.Offences.Add(offence);
                db.SaveChanges();
                return View(db.Offences.ToList());
            }
            return View(offence);
        }

        [HttpPost]
        public ActionResult Update_Rules(Offence offence,int Id)
        {
            if (ModelState.IsValid)
            {
                var data = (from o in db.Offences where o.Id == Id select o).SingleOrDefault();
                data.Offence_name = offence.Offence_name;
                data.Fine = offence.Fine;
                db.SaveChanges();
            }
            return RedirectToAction("Rules");
        }

        [HttpPost]
        public ActionResult Delete_Rules(int Id)
        {
            var data = (from o in db.Offences where o.Id == Id select o).SingleOrDefault();
            db.Offences.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Rules");
        }
    }
}