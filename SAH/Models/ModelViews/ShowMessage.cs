using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowMessage
    {
        public MessageDto Message { get; set; }
        //Chat associated with message
        public ChatDto MessageChat { get; set; }
        //User (or sender) associated with chat
        public UserDto MessageSender { get; set; }
    }
}