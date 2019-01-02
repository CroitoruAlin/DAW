using Facebook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{

    public class UsersController : Controller
    {
        ApplicationDbContext applicationDbContext;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UsersController()
        {
            applicationDbContext = new ApplicationDbContext();
        }
        [HttpPut]
        public ActionResult AcceptRequest(int id)
        {
            Friendship request = applicationDbContext.Friendships.Find(id);
            request.Approved = true;
            applicationDbContext.SaveChanges();
            return RedirectToAction("Friends");
        }
        [HttpGet]
        public ActionResult Friends()
        {
            IEnumerable<Friendship> friendrequest = applicationDbContext.Friendships.Include("User2").Include("User1").AsEnumerable().
                Where(f => f.User2.Id == User.Identity.GetUserId() && f.Approved ==false
             ).ToList();
            IEnumerable<Friendship> friends1 = applicationDbContext.Friendships.Include("User2").Include("User1").AsEnumerable().
                Where(f => f.User2.Id == User.Identity.GetUserId() && f.Approved == true
             ).ToList();
            IEnumerable<Friendship> friends2 = applicationDbContext.Friendships.Include("User2").Include("User1").AsEnumerable().
               Where(f => f.User1.Id == User.Identity.GetUserId() && f.Approved == true
            ).ToList();

            ViewBag.friends1 = friends1;
            ViewBag.friends2 = friends2;
            ViewBag.requests = friendrequest;
            return View();
        }


        [HttpGet]
        public ActionResult Edit()
        {
            if (User.Identity.IsAuthenticated) {
                
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateFriendRequest(string id)
        {
            if(id != User.Identity.GetUserId() && id !=null)
            {
                ApplicationUser user1 = applicationDbContext.Users.Find(User.Identity.GetUserId());
                ApplicationUser user2 = applicationDbContext.Users.Find(id);
                Friendship friendship = new Friendship(user1, user2, false);
                applicationDbContext.Friendships.Add(friendship);
                applicationDbContext.SaveChanges();
                return RedirectToAction("PersonalPage",new { Id = id });
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult ChangePassword(ResetPasswordViewModel reset)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = applicationDbContext.Users.Find(userId);
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(reset.Password);
                applicationDbContext.SaveChanges();
                return RedirectToAction("PersonalPage", "Users");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPut]
        public ActionResult Edit(EditModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser applicationUser = applicationDbContext.Users.Find(userId);
                if (model.Firstname != null && model.Firstname.Length > 1 && !model.Firstname.Contains(' ')) {
                    applicationUser.UserDetails.Firstname = model.Firstname;
                        }
                if (model.Firstname != null && model.Firstname.Length > 1 && !model.Firstname.Contains(' '))
                {
                    applicationUser.UserDetails.Firstname = model.Firstname;
                }
                if (model.Lastname != null && model.Lastname.Length > 1 && !model.Lastname.Contains(' '))
                {
                    applicationUser.UserDetails.Lastname = model.Lastname;
                }
                applicationUser.Email = model.Email;
                applicationDbContext.SaveChanges();


                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: Users
        public ActionResult PersonalPage(String Id)
        {
            var personalInfo = from user in applicationDbContext.Users
                               where user.Id == Id
                               select new PersonalModel
                               {
                                   Firstname = user.UserDetails.Firstname,
                                   Lastname = user.UserDetails.Lastname,
                                   Email = user.Email,
                                   check = user.UserDetails.privateProfile,
                                   id = Id
                               };
            IEnumerable<Album> albums;
            if (Id != null && !Id.Equals(""))
            {
                List<Friendship> friends1 = applicationDbContext.Friendships.Include("User2").Include("User1").AsEnumerable().
                           Where(f => f.User2.Id == User.Identity.GetUserId() && f.User1.Id == Id
                        ).ToList();
                List<Friendship> friendship1 = applicationDbContext.Friendships.Include("User1").Include("User2").AsEnumerable().Where(f => f.User2.Id == Id &&
                f.User1.Id == User.Identity.GetUserId()).ToList();
                 albums = applicationDbContext.Albums.Include("Owner").Where(album => album.Owner.Id == Id).ToList();

                foreach (PersonalModel model in personalInfo)
                {
                    ViewBag.flag2 = (friends1.Count > 0) || (friendship1.Count > 0);
                    if (model.check == false)
                    {
                        ViewBag.albums = albums;
                        ViewBag.flag = false;

                        return View(model);
                    }
                    else
                    {
                        if (ViewBag.flag2 == true)
                        {
                            ViewBag.albums = albums;
                            ViewBag.flag = true;
                            return View(model);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                
            }
                String userId = User.Identity.GetUserId();
                personalInfo = from user in applicationDbContext.Users
                                   where user.Id == userId
                                   select new PersonalModel
                                   {
                                       Firstname = user.UserDetails.Firstname,
                                       Lastname = user.UserDetails.Lastname,
                                       Email = user.Email,
                                       check = user.UserDetails.privateProfile,
                                       id = userId
                                   };
                PersonalModel personal = null;
                foreach (PersonalModel model in personalInfo)
                {
                    personal = new PersonalModel(model.Firstname, model.Lastname, model.Email, model.check,model.id);
                }
             albums = applicationDbContext.Albums.Include("Owner").Where(album => album.Owner.Id == userId).ToList();

            ViewBag.albums = albums;
                return View(personal);
           
        }
        public FileContentResult UsersPhotos(String userId)
        {

            if (userId == null)
            {
                byte[] image = GetImage("Blank-profile.png");
                return File(image, "image/png");
            }
            var bd = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = bd.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user.UserPhoto != null)
            {
                byte[] image = GetImage(user.UserPhoto);
                return File(image, "image/png");
            }
            return File(GetImage("Blank-profile.png"), "image/png");
        }
        private byte[] GetImage(String Name)
        {
            string fileName = HttpContext.Server.MapPath(@"~/Content/Images/"+Name);
            FileInfo fileInfo = new FileInfo(fileName);
            long length = fileInfo.Length;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fileStream);
            byte[] image = reader.ReadBytes((int)length);
            return image;
        }

        [HttpPost]
        public ActionResult SetPrivateProfile()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            var user = applicationDbContext.Users.Find(userId);
            if(user.UserDetails.privateProfile)
            {
                user.UserDetails.privateProfile = false;
            }
            else
            {
                user.UserDetails.privateProfile = true;
            }
            applicationDbContext.SaveChanges();
            return RedirectToAction("PersonalPage");
        }
    }
}