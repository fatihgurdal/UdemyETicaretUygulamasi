using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UdemyETicaret.Models.Message
{
    public class RenderMessageModel
    {
        public List<DB.Messages> Messages { get; set; }
        public int Count { get; set; }
    }
}