using Facebook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{
    public class CommentsController : Controller
    {
        ApplicationDbContext applicationDbContext;

        public CommentsController()
        {
            applicationDbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public ActionResult Create(Comment comment)
        {
           if(User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = applicationDbContext.Users.Find(userId);
                comment.Owner = user;
                comment.Date = System.DateTime.Now;
                comment.Accepted = false;
                Post post = applicationDbContext.Posts.Find(comment.Post.Id);
                comment.Post = post;
                applicationDbContext.Comments.Add(comment);
                applicationDbContext.SaveChanges();
                return RedirectToAction("Index", "Posts", new
                {
                    comment.Post.Id
                });
            }
            return RedirectToAction("Index", "Home");
        }
     

    }
}