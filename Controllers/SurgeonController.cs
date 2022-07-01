using Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class SurgeonController : Controller
    {
        readonly Project_databaseEntities db = new Project_databaseEntities();
        // GET: Surgeon
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(int id = 0)
        {
            var d = db.Drivers.ToList().Skip(id * 10).Take(10);
            return View(d);
        }
        [HttpGet]
        public ActionResult Search_Driver()
        {
            string name = this.Request.Params["name"];
            string regNumber = this.Request.Params["regNumber"];
            if (name.Equals(""))
            {
                var result = db.Drivers.Where(x => x.Driving_license_number.ToString().Contains(regNumber)).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (regNumber.Equals(""))
            {
                var result = db.Drivers.Where(x => (x.First_name+" "+ x.Last_name).Contains(name)).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = db.Drivers.Where(x => (x.First_name + " " + x.Last_name).Contains(name) || x.Driving_license_number.ToString().Contains(regNumber)).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Driver_Details(int id)
        {
            Driver dr = (from d in db.Drivers where d.Id == id select d).SingleOrDefault();
            var offence = (from o in db.Offences select o);
            var offence_details = (from ol in db.Offence_info where ol.Driver_id == dr.Id select ol).ToList();
            //join o in db.Offences on ol.Offence_id equals o.Id
            //join s in db.Surgeons on ol.Surgeon_id equals s.Id
            // join d in db.Drivers on ol.Driver_id equals d.Id

            var data = (dr, offence, offence_details);
            return View(data);
        }
        [HttpPost]
        public ActionResult Add_Offence(Offence offence,int id)
        {
            Driver driver = (from d in db.Drivers where d.Id == id select d).SingleOrDefault();
            /*Offence_info newOffence = new Offence_info();
            newOffence.Offence = offence;
            newOffence.Payment_status = "false";
            newOffence.Occuring_date = DateTime.Now.ToString();
            newOffence.Driver = driver;
            newOffence.Surgeon = new Surgeon { Id = 20000, Password = "1020304050", Zone = "Farmgate-1" };*/
            db.Offence_info.Add(new Offence_info { Occuring_date = DateTime.Now.ToString(), Payment_status = "false", Driver = driver, Offence = offence, Surgeon = new Surgeon { Id = 20000, Password = "1020304050", Zone = "Farmgate-1" } });
            //_ = db.Offence_info.Add(newOffence);
            _ = db.SaveChanges();
            return RedirectToAction("Search");
        }

        public ActionResult Rules()
        {
            var rules = db.Offences.ToList();
            return View(rules);
        }

        public ActionResult History(int id=0)
        {
            //id need to be dynamic
            var history = (from oi in db.Offence_info where oi.Surgeon_id == 21002 select oi).ToList();
            return View(history);
        }
    }
}