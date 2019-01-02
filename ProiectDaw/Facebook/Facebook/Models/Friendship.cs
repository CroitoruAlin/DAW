using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class Friendship
    {

        

        public Friendship(ApplicationUser user1, ApplicationUser user2, bool v)
        {
            User1 = user1;
            User2 = user2;
            Approved = v;
        }

  

        public Friendship(int id, ApplicationUser user1, ApplicationUser user2, bool approved) 
        {
            Id = id;
            User1 = user1;
            User2 = user2;
            Approved = approved;
        }

        public Friendship()
        {
        }

        [Key]
        public int Id { get; set; }
        public virtual ApplicationUser User1 { get; set; }
        public virtual ApplicationUser User2 { get; set; }
        public bool Approved { get; set; }
    }
}