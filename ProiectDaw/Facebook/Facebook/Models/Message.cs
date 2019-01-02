using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facebook.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public ApplicationUser Owner { get; set; }
        public ApplicationUser Receiver { get; set; }
        public DateTime CreationDate { get; set; }
    }
}