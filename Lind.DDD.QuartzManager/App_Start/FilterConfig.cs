using System.Web;
using System.Web.Mvc;

namespace Lind.DDD.QuartzManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}