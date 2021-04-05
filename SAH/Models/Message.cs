using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        /*[ForeignKey("Chat")]
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }*/
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
        public DateTime DateSent { get; set; }
        public string Content  { get; set; }
    }

    public class MessageDto
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public string DateSent { get; set; }
        public string Content { get; set; }
    }
}