using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public DateTime DateSent { get; set; }
        public string Content { get; set; }
    }

    public class MessageDto
    {
        [DisplayName("Message Id")]
        public int MessageId { get; set; }
        [DisplayName("Sender Id")]
        public string SenderId { get; set; }
        [DisplayName("Chat Id")]
        public int ChatId { get; set; }
        [DisplayName("Date Sent")]
        public DateTime DateSent { get; set; }
        [DisplayName("Message Content")]
        public string Content { get; set; }
    }
}