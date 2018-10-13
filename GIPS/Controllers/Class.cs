using System;
namespace GIPS.Controllers
{
    public class UserClass
    {
        public Guid _id { get; set; }
        public string Userid { get; set; }
    }
    public class UsageClass
    {
        public Guid _id { get; set; }
        public string Usage { get; set; }
    }

    public class UsageLog
    {
        public Guid _id { get; set; }
        public string Userid { get; set; }
        public DateTime Date { get; set; }
    }
}
