using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class UserDetails
    {
        [Key]
        public int DetailsId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool privateProfile { get; set; }

        public UserDetails(string firstname, string lastname,bool flag)
        {
            Firstname = firstname;
            Lastname = lastname;
            privateProfile = flag;
        }

        public UserDetails()
        {
            privateProfile = false;
        }
    }
}