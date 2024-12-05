using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeister.Data
{
    internal class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Message latestMessage { get; set; }
    }
}
