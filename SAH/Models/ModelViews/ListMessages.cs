using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ListMessages
    {
        public IEnumerable<MessageDto> Messages { get; set; }
        //Chat associated with message
        public ChatDto Chat { get; set; }   
    }
}