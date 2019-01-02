using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set;}
        public virtual ApplicationUser Owner { get; set; }
        public DateTime Date { get; set; }
        public virtual Post Post { get; set; }
        public string Content { get; set; }
        public bool Accepted { get; set; }
    }
}