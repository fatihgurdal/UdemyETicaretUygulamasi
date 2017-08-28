using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UdemyETicaret.Models.Account
{
    public class ProfilModels
    {
        public DB.Members Members { get; set; }
        public List<DB.Addresses> Addresses { get; set; }
        public DB.Addresses CurrentAddress { get; set; }
    }
}