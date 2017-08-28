using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UdemyETicaret.Models.Message
{
    public class SendMessageModel
    {
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public int ToUserId { get; set; }
    }
}