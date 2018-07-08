using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBEntity;
using TBRepo;

namespace TravelBlog.Controllers
{
    public class AdminController : Controller
    {
        private AdminRepository repo = new AdminRepository();
        private UserRepository urepo = new UserRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            bool valid = repo.Validate(admin);
            if (valid)
            {
                Session["AdminEmail"] = admin.Email;
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Invalid username or password");
            }
        }

        public ActionResult Profile()
        {
            Admin admin = new Admin();
            admin = this.repo.Info(Session["AdminEmail"].ToString());
            return View(admin);
        }

        [HttpGet]
        public ActionResult Edit(Admin admin)
        {
            admin = this.repo.Info(Session["AdminEmail"].ToString());
            return View(admin);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            Admin admin = new Admin();
            admin.Email = Session["AdminEmail"].ToString();
            admin.FirstName = collection["fname"];
            admin.LastName = collection["lname"];
            admin.Phone = collection["phone"];
            admin.Division = collection["division"];
            admin.Address = collection["address"];
            this.repo.Update(admin);
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public ActionResult ChangePassword(Admin admin)
        {
            admin = this.repo.Info(Session["AdminEmail"].ToString());
            return View(admin);
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            string newPass, confirmNewPass;
            User user = new User();
            user.Email = Session["Email"].ToString();
            user.Password = collection["cpass"];
            newPass = collection["npass"];
            confirmNewPass = collection["cnpass"];
            bool correctOldPass = repo.CheckOldPass(user.Email, user.Password);
            if (correctOldPass)
            {
                if (newPass == confirmNewPass)
                {
                    this.repo.ChangePass(Session["AdminEmail"].ToString(), newPass);
                }
            }
            return RedirectToAction("ChangePassword");
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(FormCollection collection)
        {
            Admin admin = new Admin();
            admin.FirstName = collection["fname"];
            admin.LastName = collection["lname"];
            admin.Email = collection["email"];
            admin.Phone = collection["phone"];
            admin.Division = collection["division"];
            admin.Address = collection["address"];
            admin.Password = collection["password"];
            bool existAdmin = repo.IsExistAdmin(admin.Email);
            if (!existAdmin)
            {
                this.repo.Insert(admin);
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Admin with this email is already exist");
            }
        }

        public ActionResult UserList()
        {
            return View(this.urepo.GetAllUser());
        }

        [HttpGet]
        public ActionResult UserDetails(int id)
        {
            User u = this.urepo.GetUser(id);
            return View(u);
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            User u = this.urepo.GetUser(id);
            return View(u);
        }

        [HttpPost][ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            this.urepo.DeleteUser(id);
            return RedirectToAction("UserList");
        }
    }
}