﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; private set; }
        public bool WasEdited { get; set; }

        public Message(string content)
        {
            Content = content;
            CreatedAt = DateTime.Now.ToUniversalTime();
            WasEdited = false;
        }
    }
}
