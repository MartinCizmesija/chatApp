using System;
using System.Collections.Generic;

namespace chatApp.Models
{
    public class Chat
    {
        public string text;
        public string protocol;
        public List<Tuple<string, string>> chatLog = new List<Tuple<string, string>>();

        public Chat (string protocol, List<Tuple<string, string>> log)
        {
            this.chatLog = log;
            this.protocol = protocol;
        }
    }
}
