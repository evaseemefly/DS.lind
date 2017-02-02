using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Lind.DDD.OAuthDemo.Controllers
{
    public class OAuthController : Controller
    {
        static int minuteExpire = 1;
        static Cache OAuthDB = System.Web.HttpRuntime.Cache;
         /// <summary>
        /// 获取未授权的Request Token服务地址；
        /// </summary>
        /// <returns></returns>
        public ActionResult GetToken(string returnUrl, string appId)
        {
            if (appId != "zzl")
                return Content("appId不存在");
            var token = Guid.NewGuid().ToString();
            OAuthDB.Insert(token, 1, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minuteExpire));
            return Redirect(returnUrl + "?token=" + token);
        }

        /// <summary>
        /// 获取用户授权的Request Token服务地址；
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAuthorization(string returnUrl, string token)
        {
            if (OAuthDB.Get(token) == null)
                return Content("token不存在");
            var auth = Guid.NewGuid().ToString();
            OAuthDB.Insert(auth, 1, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minuteExpire));
            return Redirect(returnUrl + "?authorization=" + auth);
        }
        /// <summary>
        /// 用授权的Request Token换取Access Token的服务地址；
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAccessToken(string returnUrl, string authorization)
        {
            if (OAuthDB.Get(authorization) == null)
                return Content("authorization不存在");
            return Redirect(returnUrl + "?accessToken=1");
        }
    }
}
