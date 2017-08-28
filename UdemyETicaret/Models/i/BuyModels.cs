using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UdemyETicaret.Models.i
{
    public class BuyModels
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public decimal TotelPrice { get; set; }
        public string OrderStatus { get; set; }
        public DB.Members Member { get; set; }
    }
}