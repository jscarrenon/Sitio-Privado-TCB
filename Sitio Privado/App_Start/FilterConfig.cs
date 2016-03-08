﻿using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute()); //Privatiza todo por default
        }
    }
}