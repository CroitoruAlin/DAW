using Facebook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext applicationDbContext;

        public GroupsController()
        {
            applicationDbContext = new ApplicationDbContext();
        }

        // GET: Groups
        public ActionResult Index()
        {
            
                string userId = User.Identity.GetUserId();
                ApplicationUser user = applicationDbContext.Users.Find(userId);
                IEnumerable<Group> groups = applicationDbContext.GroupUsers.Include("User").Include("Group").
                    Where(m => m.User.Id == userId).Select(m => m.Group);
            IEnumerable<Request> requests = applicationDbContext.Requests.Include("Group").Include("User").
                Where(m => m.Group.Founder.Id == userId && m.Accepted == false).ToList(); ;
            ViewBag.requests = requests;
                return View(groups);
            

        }
        public ActionResult Show(int Id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            Group group = applicationDbContext.Groups.Include("Founder").Where(x => x.Id == Id).First();
            GroupUser groupUser = applicationDbContext.GroupUsers.Include("Group").Include("User").
                Where(m => m.User.Id == user.Id && m.Group.Id == group.Id).FirstOrDefault();
            if (groupUser != null)
            {
                IEnumerable<GroupMessage> messages = applicationDbContext.GroupMessages.Include("Group").Include("Owner").
                    Where(m => m.Group.Id == Id).ToList();
                ViewBag.messages = messages;
                return View(group);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Index(string Name)
        {
            IEnumerable<Group> groups1 = applicationDbContext.Groups.
                Where(m => m.Name.Contains(Name)).ToList();
            ViewBag.groups = groups1;

            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            IEnumerable<Group> groups = applicationDbContext.GroupUsers.Include("User").Include("Group").
                Where(m => m.User.Id == userId).Select(m => m.Group);
            IEnumerable<Request> requests = applicationDbContext.Requests.Include("Group").Include("User").
                Where(m => m.Group.Founder.Id == userId && m.Accepted == false).ToList(); ;
            ViewBag.requests = requests;
            return View(groups);
            
            
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Group group)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            group.Founder = user;
            applicationDbContext.Groups.Add(group);
            GroupUser groupUser = new GroupUser();
            groupUser.Group = group;
            groupUser.User = user;
            applicationDbContext.GroupUsers.Add(groupUser);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CreateMessage(GroupMessageModel messageModel)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            GroupMessage groupMessage = new GroupMessage();
            groupMessage.Owner = user;
            Group group = applicationDbContext.Groups.Find(messageModel.Id);
            groupMessage.Group = group;
            groupMessage.Content = messageModel.Message;
            groupMessage.CreationDate = System.DateTime.Now;
            applicationDbContext.GroupMessages.Add(groupMessage);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Show", new { Id = messageModel.Id });
        }
        [HttpPost]
        public ActionResult SendRequest(int Id)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            Group group = applicationDbContext.Groups.Include("Founder").Where(m => m.Id == Id).FirstOrDefault();
            Request request = new Request();
            request.Accepted = false;
            request.Group = group;
            request.User = user;
            applicationDbContext.Requests.Add(request);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Accept(int Id)
        {
            Request request = applicationDbContext.Requests.Include("Group").Include("User").Where(m => m.Id == Id).FirstOrDefault();
            GroupUser groupUser = new GroupUser();
            groupUser.Group = request.Group;
            groupUser.User = request.User;
            applicationDbContext.GroupUsers.Add(groupUser);
            request.Accepted = true;
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}