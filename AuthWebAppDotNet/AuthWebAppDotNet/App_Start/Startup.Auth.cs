using System;
using System.Configuration;

// The following using statements were added for this sample
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Notifications;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Identity.Client;
using AuthWebAppDotNet.Helpers;


namespace AuthWebAppDotNet
{
    public partial class Startup
    {
        // App config settings
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        // B2C policy identifiers
        public static string SusiPolicyId = ConfigurationManager.AppSettings["ida:SusiPolicyId"];
        public static string PasswordResetPolicyId = ConfigurationManager.AppSettings["ida:PasswordResetPolicyId"];
        public static string EditProfile = ConfigurationManager.AppSettings["ida:EditProfile"];



        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Configure OpenID Connect middleware for each policy
            app.UseOpenIdConnectAuthentication(CreateOptionsFromPolicy(PasswordResetPolicyId));
            app.UseOpenIdConnectAuthentication(CreateOptionsFromPolicy(SusiPolicyId));
            app.UseOpenIdConnectAuthentication(CreateOptionsFromPolicy(EditProfile));
            
           
           
        
        }

        // Used for avoiding yellow-screen-of-death TODO
        private Task AuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();

            if (notification.ProtocolMessage.ErrorDescription != null && notification.ProtocolMessage.ErrorDescription.Contains("AADB2C90118"))
            {
                // If the user clicked the reset password link, redirect to the reset password route
                notification.Response.Redirect("/Account/ResetPassword");
            }
            else if (notification.Exception.Message == "access_denied")
            {
                // If the user canceled the sign in, redirect back to the home page
                notification.Response.Redirect("/");
            }
            else
            {
                notification.Response.Redirect("/Home/Error?message=" + notification.Exception.Message);
            }

            return Task.FromResult(0);
        }

        private async Task<int>  OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            //If you wanted to keep some local state in the app(like a db of signed up users),
            // you could use this notification to create the user record if it does not already
            // exist.i





            string userID = notification.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var _roles = await RolesProvider.GetRoleByUserId(userID, await TokenProviders.getTokenAsync());

            foreach (var role in _roles)
            {
                if (role.Equals("Application Administrator"))
                {
                    Claim claim = new Claim(notification.AuthenticationTicket.Identity.RoleClaimType, "Admin");
                    notification.AuthenticationTicket.Identity.AddClaim(claim);
                }

                if (role.Equals("Application Developer"))
                {
                    Claim claim = new Claim(notification.AuthenticationTicket.Identity.RoleClaimType, "Developer");
                    notification.AuthenticationTicket.Identity.AddClaim(claim);
                }
            }



            int r = await Task.FromResult(0);
            return r;
        }

        private OpenIdConnectAuthenticationOptions CreateOptionsFromPolicy(string policy)
        {
            return new OpenIdConnectAuthenticationOptions
            {
                // For each policy, give OWIN the policy-specific metadata address, and
                // set the authentication type to the id of the policy
                MetadataAddress = String.Format(aadInstance, tenant, policy),
                AuthenticationType = policy,

                // These are standard OpenID Connect parameters, with values pulled from web.config
                ClientId = clientId,
                RedirectUri = redirectUri,
                PostLogoutRedirectUri = redirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = AuthenticationFailed,
                    SecurityTokenValidated = OnSecurityTokenValidated,
                },
                Scope = "openid",
                ResponseType = "id_token",

                // This piece is optional - it is used for displaying the user's name in the navigation bar.
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                },
            };
        }


        
       
    }
}