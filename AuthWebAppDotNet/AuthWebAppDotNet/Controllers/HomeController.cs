using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using AuthWebAppDotNet.Helpers;
using System.Threading.Tasks;

namespace AuthWebAppDotNet.Controllers
{
    
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        [Authorize(Roles = "Developer")]
        
        public  ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.Only developer can access";
            //Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);

            
            return View();
        }
        // You can use the PolicyAuthorize decorator to execute a certain policy if the user is not already signed into the app.
        [Authorize(Roles = "Admin,Developer")]
        public ActionResult Claims()
        {
           
            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);                 

            ViewBag.DisplayName = displayName != null ? displayName.Value : string.Empty;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SecuredPage()
        {
            ViewBag.DisplayName = "This is secured page only admin can access";
            return View();
        }
        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }

    }
}