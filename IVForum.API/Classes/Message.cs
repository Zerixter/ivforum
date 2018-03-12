using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Classes
{
    public class Message
    {
        public static object GetMessage(string text)
        {
            var Message = new
            {
                Message = text
            };
            return Message;
        }
    }
}
