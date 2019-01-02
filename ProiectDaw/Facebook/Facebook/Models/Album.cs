using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Album's Title")]
        public string Name { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}