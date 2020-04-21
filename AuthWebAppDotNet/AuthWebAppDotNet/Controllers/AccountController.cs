using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;  
namespace AuthWebAppDotNet.Controllers
{
    public class AccountController : Controller
    {
        
        public void Login()
        {
            if (!Request.IsAuthenticated)
            {
                // To execute a policy, you simply need to trigger an OWIN challenge.
                // You can indicate which policy to use by specifying the policy id as the AuthenticationType

                AuthenticationProperties aup = new AuthenticationProperties() { RedirectUri = "/" };

                HttpContext.GetOwinContext().Authentication.Challenge(aup, Startup.SusiPolicyId);

                //var identity = ClaimsPrincipal.Current.Identities.First();
                //var extraClaim = new Claim(identity.RoleClaimType, "Extra");
                //identity.AddClaim(extraClaim);
                //identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));


            }
            return;
        }
        public void ResetPassword()
        {
            if (Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties() { RedirectUri = "/" }, Startup.PasswordResetPolicyId);
            }
        }

        public void EditProfile()
        {
            if (Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties() { RedirectUri = "/" }, Startup.EditProfile);
            }
        }

        public void Logout()
        {
            // To sign out the user, you should issue an OpenIDConnect sign out request.
            if (Request.IsAuthenticated)
            {
                IEnumerable<AuthenticationDescription> authTypes = HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes();
                HttpContext.GetOwinContext().Authentication.SignOut(authTypes.Select(t => t.AuthenticationType).ToArray());
                Request.GetOwinContext().Authentication.GetAuthenticationTypes();
            }

        }
    }
}