using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.Plugins
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}