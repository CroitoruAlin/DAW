using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{
    public class UsersController : Controller
    {
        [Authorize(Roles ="Administrator")]
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
    }
}