using Facebook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult PersonalPage()
        {
            
            return RedirectToAction("PersonalPage", "Users");
        }
        [HttpPost]
        public ActionResult Index(ApplicationUser model)
        {
            var users = from user in applicationDbContext.Users where user.UserDetails.Firstname.ToLower().Contains(model.UserName.ToLower())
                        || user.UserDetails.Lastname.ToLower().Contains(model.UserName.ToLower())
                        select new
                        {
                            ID = user.Id,
                            Firstname = user.UserDetails.Firstname,
                            Lastname = user.UserDetails.Lastname
                        };
            IEnumerable<SearchModel> searches = from user in users.AsEnumerable()
                                                select new SearchModel(
                                                    user.ID,
                                                    user.Firstname,
                                                    user.Lastname
                                                    );
            ViewBag.users = searches;
            return View();
        }
     

    }
}