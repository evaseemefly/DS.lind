using Lind.DDD.CQRS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.CQRS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string message)
        {
            ViewBag.Message = message ?? "您可以单击“建立订单”或者“确认订单”";

            return View();
        }
        public ActionResult Create()
        {
            var order = new Order();
            //插入数据库
            //repository
            //执行事件
            order.Create();
            return RedirectToAction("Index", new { message = "建立了订单" });
        }

        public ActionResult Confirm()
        {
            var order = new Order();
            //更新数据库
            //repository
            //执行事件
            order.Confirm();
            return RedirectToAction("Index", new { message = "确认了订单" });
        }
        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }
    }
}
