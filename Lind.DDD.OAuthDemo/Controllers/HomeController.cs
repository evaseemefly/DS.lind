using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.OAuthDemo.Controllers
{
    public class HomeController : Controller
    {


        [Lind.DDD.OAuthDemo.Filters.OAuthFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}
