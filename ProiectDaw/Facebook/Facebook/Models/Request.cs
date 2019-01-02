using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
        public bool Accepted { get; set; }
    }
}