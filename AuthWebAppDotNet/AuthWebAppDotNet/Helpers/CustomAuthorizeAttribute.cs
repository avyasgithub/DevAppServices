using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AuthWebAppDotNet.Helpers
{
    public class CustomAuthorize:AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        
    }
}