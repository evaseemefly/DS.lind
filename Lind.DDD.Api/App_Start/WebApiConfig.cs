using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Lind.DDD.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Restful", id = RouteParameter.Optional }
            );

            // api authority
        config.Filters.Add(new Lind.DDD.Authorization.Api.ApiValiadateFilter());

            // api cors
           //  config.Filters.Add(new Lind.DDD.Authorization.Api.CorsFilter("*"));

            // 这行在使用FormUrlEncodedContent提交数据时不能加，否则出问题
            //  config.Formatters.Clear();
            //  config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
        }
    }
}
