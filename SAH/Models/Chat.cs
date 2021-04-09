﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel;


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
        [DisplayName("Chat Id")]
        public int ChatId { get; set; }
        [DisplayName("Subject")]
        public string Subject { get; set; }
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }
    }
}