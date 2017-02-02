using Lind.DDD.LindPlugins;
using Lind.DDD.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.SSODemo.Controllers
{
    public class HomeController : Controller
    {
        //static List<PluginModel> PluginList = new List<PluginModel>();
        //static HomeController()
        //{
        //    PluginList.Add(new PluginModel { ModuleName = "project", TypeDescription = "手机端", TypeName = "H5" });
        //    PluginList.Add(new PluginModel { ModuleName = "project", TypeDescription = "PC端", TypeName = "PC" });
        //}
        /// <summary>
        /// 业务网站
        /// </summary>
        /// <returns></returns>
        [SSOActionFilter]
        public ActionResult Index(string module)
        {
            // var model=PluginManager.Resolve<IShop>("module").GetModel();
            //   return View(model);
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            SSOManager.ExitSSO();
            return RedirectToAction("Index");
        }

        public ContentResult FillWindow(string url)
        {
            return Content(Utils.HttpHelper.Get(url).Content.ReadAsStringAsync().Result);
        }
    }
}
