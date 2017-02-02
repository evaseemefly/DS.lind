using Lind.DDD.Authorization.Mvc;
using Lind.DDD.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.SSODemo.Controllers
{

    /// <summary>
    /// SSO服务器的相关方法
    /// </summary>
    public class CommonController : Controller
    {

        /// <summary>
        /// 全站统一登陆
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            bool isPass = form["UserName"] == "zzl" && form["Password"] == "zzl";
            if (isPass)
            {
                return SSOManager.LoginSSO("1","zzl",form["BackUrl"]);
            }
            return Content("抱歉，帐号或密码有误！请在Web.config中配置帐号密码！");

        }

        /// <summary>
        /// 当前通过sso授权的用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrentUserList()
        {
            return View(SSOManager.Instance.GetCacheTable());
        }
    }
}
