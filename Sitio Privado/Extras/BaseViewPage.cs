using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Extras
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual Usuario Usuario
        {
            get { return new Usuario(base.User as ClaimsPrincipal); }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual Usuario Usuario
        {
            get { return new Usuario(base.User as ClaimsPrincipal); }
        }
    }
}