using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }
        public string Subject { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<User> Users { get; set; }
    }
    public class ChatDto
    {
        public int ChatId { get; set; }
        public string Subject { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}