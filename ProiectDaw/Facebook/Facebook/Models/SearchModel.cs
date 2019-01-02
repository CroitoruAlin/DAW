using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class SearchModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public SearchModel()
        {
        }

        public SearchModel(string id, string firstname, string lastname)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
        }
    }
}