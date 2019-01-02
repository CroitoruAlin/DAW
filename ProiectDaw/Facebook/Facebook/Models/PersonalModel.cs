using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class PersonalModel
    {
        public PersonalModel(string firstname, string lastname, string email,bool value,string id)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            check = value;
            this.id = id;
        }

        public PersonalModel()
        {
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool check {get; set;}
        public string id { get; set; }
    }
}