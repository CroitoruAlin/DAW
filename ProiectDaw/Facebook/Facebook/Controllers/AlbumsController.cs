using Facebook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Controllers;

namespace Facebook.Controllers
{
    public class AlbumsController : Controller
    {
        ApplicationDbContext applicationDb;


        public AlbumsController()
        {
            applicationDb = new ApplicationDbContext();
        }
        [HttpGet]
        public ActionResult Index(int Id)
        {
            Album album = applicationDb.Albums.Include("Posts").Include("Owner").Where(a => a.Id == Id).First();
            string userId = User.Identity.GetUserId();
            ApplicationUser owner = album.Owner;
            ApplicationUser user = applicationDb.Users.Find(userId);
            if (User.Identity.IsAuthenticated && (album.Owner.UserDetails.privateProfile==false || Utils.verifyFriendship(user,owner )
                || userId == owner.Id))
            {
                
                ViewBag.flag = album.Owner.Id == User.Identity.GetUserId();
                return View(album);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated) {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Create(string name)
        {
            if(User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = applicationDb.Users.Where(u => u.Id == userId).First();
                Album album = new Album();
                album.Name = name;
                album.Owner = user;
                applicationDb.Albums.Add(album);
                applicationDb.SaveChanges();
                return RedirectToAction("PersonalPage", "Users");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}