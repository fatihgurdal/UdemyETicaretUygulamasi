using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UdemyETicaret.Models.Message
{
    public class iModels
    {
        public List<System.Web.Mvc.SelectListItem> Users { get; set; }
        public List<DB.Messages> Messages { get; set; }
    }
}