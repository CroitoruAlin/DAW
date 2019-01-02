using Facebook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext applicationDbContext;

        public MessagesController()
        {
            applicationDbContext = new ApplicationDbContext();
        }

        // GET: Message
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
               
                    string userId = User.Identity.GetUserId();
                    IEnumerable<Message> messages = applicationDbContext.Messages.Include("Owner").Include("Receiver").
                        Where(m => m.Receiver.Id == userId).ToList();
                ViewBag.userId = userId;
                    return View(messages);
             
            }
            return RedirectToAction("Index", "Home");
            
        }
        [HttpGet]
        public ActionResult Create(string Id)
        {
            Message message = new Message();
            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            message.Owner = user;
            ApplicationUser receiver = applicationDbContext.Users.Find(Id);
            message.Receiver = receiver;
            return View(message);
        }
        [HttpPost]
        public ActionResult Create(Message message)
        {
            ApplicationUser owner = applicationDbContext.Users.Find(message.Owner.Id);
            ApplicationUser receiver = applicationDbContext.Users.Find(message.Receiver.Id);
            message.Owner = owner;
            message.Receiver = receiver;
            message.CreationDate = System.DateTime.Now;
            applicationDbContext.Messages.Add(message);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}