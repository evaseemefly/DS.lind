using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.OAuthDemo.Filters
{
    public class OAuthFilter : AuthorizeAttribute
    {
        const string DOMAIN = "http://localhost:5076";
        const string TOKENURL = DOMAIN + "/OAuth/GetToken?";
        const string AUTHORIZATION = DOMAIN + "/OAuth/GetAuthorization?";
        const string ACCESSTOKEN = DOMAIN + "/OAuth/GetAccessToken?";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var Request = filterContext.RequestContext.HttpContext.Request;
            var nv = new System.Collections.Specialized.NameValueCollection();

            #region 第一步
            if (Request.QueryString["token"] == null
                && Request.QueryString["authorization"] == null
                && Request.QueryString["accesstoken"] == null)
            {
                nv.Add("returnUrl", Request.Url.AbsolutePath);
                nv.Add("appId", "zzl");
                filterContext.RequestContext.HttpContext.Response.Redirect(TOKENURL + nv.ToUrl());
            }
            #endregion

            #region 第二步
            if (Request.QueryString["token"] != null)
            {
                nv = new System.Collections.Specialized.NameValueCollection();
                nv.Add("returnUrl", Request.Url.AbsolutePath);
                nv.Add("token", Request.QueryString["token"]);
                filterContext.RequestContext.HttpContext.Response.Redirect(AUTHORIZATION + nv.ToUrl());
            }
            #endregion

            #region 第三步
            if (Request.QueryString["authorization"] != null)
            {
                nv = new System.Collections.Specialized.NameValueCollection();
                nv.Add("returnUrl", Request.Url.AbsolutePath);
                nv.Add("authorization", Request.QueryString["authorization"]);
                filterContext.RequestContext.HttpContext.Response.Redirect(ACCESSTOKEN + nv.ToUrl());

            }
            #endregion

            #region 第四步
            if (Request.QueryString["accesstoken"] != null)
            {
                filterContext.RequestContext.HttpContext.Session["accesstoken"] = Request.QueryString["accesstoken"];
            }
            #endregion

        }
    }
}