using System.Web.Mvc;

namespace GIPS.Controllers
{
    [RoutePrefix("static")]
    public class StaticController : Controller
    {

        public ActionResult Index()
        {
            return View("db_list");
        }

        [Route("DBList")]
        public ActionResult DBList()
        {
            return View("db_list");
        }
    }
}
