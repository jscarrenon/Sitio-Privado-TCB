﻿using Sitio_Privado.Filters;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
