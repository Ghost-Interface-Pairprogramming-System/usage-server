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

        public ActionResult DBList()
        {
            return View("db_list");
        }

        public ActionResult UserViewer()
        {
            return View("viewer_page");
        }
    }
}
