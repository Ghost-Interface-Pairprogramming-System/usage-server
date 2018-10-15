using System;
namespace GIPS.Controllers
{
    public class UserClass
    {
        public Guid ID { get; set; }
    }
    public class UsageClass
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UsageLog
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid UsageID { get; set; }
        public DateTime Date { get; set; }
    }
    public class Usage
    {
        public string Action { get; set; }
        public DateTime Date { get; set; }
    }

}
