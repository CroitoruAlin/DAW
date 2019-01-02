using Facebook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext applicationDbContext;

        public PostsController()
        {
            applicationDbContext = new ApplicationDbContext();
        }
        [HttpPut]
        public ActionResult Accept(int commentId)
        {
            Comment comment = applicationDbContext.Comments.Include("Post").Where(c =>c.Id == commentId).First();
            comment.Accepted = true;
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Posts", new { Id = comment.Post.Id });
        }
        // GET: Posts
        public ActionResult Index(int Id)
        {
            Comment comment = new Comment();
            Post post = applicationDbContext.Posts.Include("Album").Include("comments").Where(m => m.Id == Id).First();
            ApplicationUser owner = post.Album.Owner;
            string userId = User.Identity.GetUserId();
            ApplicationUser user = applicationDbContext.Users.Find(userId);
            if (User.Identity.IsAuthenticated && (post.Album.Owner.UserDetails.privateProfile == false || 
                Utils.verifyFriendship(owner, user) || userId == owner.Id))
            {
                bool flag = post.Album.Owner.Id == User.Identity.GetUserId();
                ViewBag.flag = flag;
                comment.Post = post;
                return View(comment);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Create(int albumId)
        {
            if(User.Identity.IsAuthenticated)
            {
                Album album = applicationDbContext.Albums.Find(albumId);
                Post post = new Post
                {
                    Album = album
                };
                return View(post);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Create(Post post)
        {
            if(User.Identity.IsAuthenticated)
            {
               if(Request.Files.Count > 0)
                {
                    string userId = User.Identity.GetUserId();
                    ApplicationUser applicationUser = applicationDbContext.Users.Find(userId);
                    string path = HttpContext.Server.MapPath(@"~/Content/Images");
                    DateTime currentTime = DateTime.Now;
                    string filename = applicationUser.Email + currentTime.ToString();
                    
                    filename = filename.Replace('/', '_');
                    filename = filename.Replace(' ', '_');
                    filename = filename.Replace(':', '_');
                    string fullPath = path + "\\" + filename+".jpg";
                    Album album = applicationDbContext.Albums.Find(post.Album.Id);
                    post.Path = fullPath;
                    post.CreationDate = currentTime;
                    post.Album = album;
                    
                    HttpPostedFileBase image = Request.Files["Photo"];
                    if(image.ContentLength > 0)
                    {
                        image.SaveAs(fullPath);
                    }
                    applicationDbContext.Posts.Add(post);
                    applicationDbContext.SaveChanges();
                    return RedirectToAction("PersonalPage", "Users");
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public FileContentResult GetImage(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binary = new BinaryReader(file);
            byte[] image = binary.ReadBytes((int)fileInfo.Length);
            return File(image, "image/jpg");
        }
    }
   
}