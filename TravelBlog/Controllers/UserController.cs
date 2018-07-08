using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBEntity;
using TBRepo;

namespace TravelBlog.Controllers
{
    public class UserController : UserBaseController
    {
        private TBReposetory repo = new TBReposetory();

        private UserRepository urepo = new UserRepository();

        public ActionResult Index(User user)
        {
            if (user != null)
            {
                Session["uModel"] = user;
                return View(user);
            }
            else    return View((User)Session["uModel"]);
            
        }

        public ActionResult Profile()
        {
            User user = new User();
            user =  this.urepo.Info(Session["Email"].ToString());
            return View(user);
        }
        
        [HttpGet]
        public ActionResult Edit(User user)
        {
            user = this.urepo.Info(Session["Email"].ToString());
            return View(user);
        }
       
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            User user = new User();
            user.Email = Session["Email"].ToString();
            user.FirstName = collection["fname"];
            user.LastName = collection["lname"];
            user.Phone = collection["phone"];
            user.Division = collection["division"];
            user.Address = collection["address"];
            this.urepo.Update(user);
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public ActionResult ChangePassword(User user)
        {
            user = this.urepo.Info(Session["Email"].ToString());
            return View(user);
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
            bool correctOldPass = urepo.CheckOldPass(user.Email, user.Password);
            if(correctOldPass)
            {
                if(newPass == confirmNewPass)
                {
                    this.urepo.ChangePass(Session["Email"].ToString(), newPass);
                }
            }
            return RedirectToAction("ChangePassword");
        }

        public ActionResult SignOut()
        {
            Session.Remove("Email");
            return RedirectToAction("Index", "Default");
        }
    }
}