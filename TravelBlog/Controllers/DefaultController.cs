using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBEntity;
using TBRepo;

namespace TravelBlog.Controllers
{
    public class DefaultController : Controller
    {
        private TBReposetory  repo = new TBReposetory();
        private UserRepository urepo = new UserRepository();

        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Regester()
        {
            return View("Regester");
        }

        [HttpPost]
        public ActionResult Regester(FormCollection collection)
        {
            User user = new User();
            user.FirstName = collection["fname"];
            user.LastName = collection["lname"];
            user.Email = collection["email"];
            user.Phone = collection["phone"];
            user.Division = collection["division"];
            user.Address = collection["address"];
            user.Password = collection["password"];
            
            bool existUser = repo.IsExistUser(user.Email);
            if (!existUser)
            {
                this.repo.Regestration(user);
                return RedirectToAction("Index");
            }
            else
            {
                return Content("User with this email is already exist");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            bool valid = urepo.Validate(user);
            if (valid)
            {
                Session["Email"] = user.Email;
                return RedirectToAction("Index", "User");
            }
            else
            {
                return Content("Invalid username or password");
            }
        }

        public ActionResult Divisions()
        {
            return View("Divisions");
        }
    }
}