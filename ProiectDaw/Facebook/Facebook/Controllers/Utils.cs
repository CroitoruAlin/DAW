using Facebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facebook.Controllers
{
    public class Utils
    {
        private static ApplicationDbContext applicationDb = new ApplicationDbContext();


        public static bool verifyFriendship(ApplicationUser user1, ApplicationUser user2)
        {
            
            Friendship friendship1 = applicationDb.Friendships.Include("User1").Include("User2").
                Where(f => f.User1.Id == user1.Id && f.Approved == true && f.User2.Id == user2.Id).FirstOrDefault();
            Friendship friendship2 = applicationDb.Friendships.Include("User1").Include("User2").
                Where(f => f.User2.Id == user1.Id && f.Approved == true && f.User1.Id == user2.Id).FirstOrDefault();
            return friendship1 != null || friendship2 != null;
        }
    }
}