using System;
using System.Collections.Generic;

namespace GIPS.Controllers
{
    public class UsageRequest
    {
        public string UserID { get; set; }
        public string RequestAction { get; set; }
        public DateTime Date { get; set; }
    }
    public class UsagesRequest
    {
        public string UserId { get; set; }
        public Usage[] Usages { get; set; }
    }

}