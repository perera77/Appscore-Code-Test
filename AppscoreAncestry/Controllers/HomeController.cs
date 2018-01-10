using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppscoreAncestry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Simple Search";

            return View();
        }

        public ActionResult advancedSearch()
        {
            ViewBag.Title = "Advanced Search";

            return View();
        }
    }
}
