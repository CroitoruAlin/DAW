using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class GroupMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual Group Group { get; set; }
        public DateTime CreationDate { get; set; }
    }
}