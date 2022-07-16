using Project.EF;
using Project.Models;
using System.Linq;
using System.Web.Mvc;

namespace Project.Controllers
{


    public class DriverController : Controller
    {
        readonly Project_databaseEntities1 db = new Project_databaseEntities1();

        // GET: Driver
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();// remove logout button
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = (from d in db.Drivers where d.Id.ToString() == login.Username select d).SingleOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "Incorrect Username/ID or Password";
                    return View(login);
                }
                else
                {
                    Session["role"] = "Driver";
                    Session["id"] = user.Id;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(login);
            }
        }

        public ActionResult UpdateDriver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateDriver(Driver driver)
        {
            int driverId = (int)Session["id"];
            Driver d = (from db in db.Drivers where db.Id == driverId select db).SingleOrDefault();
            d = driver;
            db.SaveChanges();
            return View(driver);
        }

        public ActionResult Rules()
        {
            var rules = db.Offences.ToList();
            return View(rules);
        }

        public ActionResult Crime()
        {
            int driverID = (int)Session["id"];
            var crime = (from d in db.Offence_info where d.Driver_id == driverID select d).ToList();
            return View(crime);

        }

        public ActionResult Vehciles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Vehciles(Vehicle vehicle)
        {
            int driverId = (int)Session["id"];
            Vehicle vehicles = (from d in db.Vehicles where d.Id == driverId select d).SingleOrDefault();
            Driver driver = db.Drivers.Where(d => d.Id == driverId).SingleOrDefault();

            db.Vehicles.Add(vehicle);
            _ = db.SaveChanges();
            return View(vehicle);
        }
    }
}