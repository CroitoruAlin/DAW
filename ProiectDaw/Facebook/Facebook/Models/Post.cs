using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public virtual Album Album { get; set; }
        public string Path { get; set; }
        public ICollection<Comment> comments { get; set; }
        public DateTime CreationDate { get; set; }
    }
}