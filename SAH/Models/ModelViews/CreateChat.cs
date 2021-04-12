using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class CreateChat
    {
        public ChatDto Chat { get; set; }
        public MessageDto FirstMessage{ get; set; }//First message of a chat - entered upon chat creation
        public UserDto Recipient { get; set; }//Send a full list of doctors as possible recipitents to be selected for chat
        public IEnumerable<UserDto> Doctors { get; set; }
    }
}