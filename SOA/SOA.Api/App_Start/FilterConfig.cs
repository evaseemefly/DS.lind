﻿using Lind.DDD.Authorization.Api;
using System;
using System.Web;
using System.Web.Mvc;

namespace SOA.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }


}