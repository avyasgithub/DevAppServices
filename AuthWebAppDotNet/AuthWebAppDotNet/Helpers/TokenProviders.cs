using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Microsoft.Identity.Client;    
using System.Threading.Tasks;
using System.Configuration;
using AuthWebAppDotNet.Helpers;
using HelperClasses;

namespace AuthWebAppDotNet.Helpers
{
    public class TokenProviders
    {
        static string aadInstance = ConfigurationManager.AppSettings["ab2c:aadinstance"];
        static string tenantID = ConfigurationManager.AppSettings["ab2c:tenantID"];
        static string clientId = ConfigurationManager.AppSettings["ab2c:clientId"];
        static string clientSecret = ConfigurationManager.AppSettings["ab2c:secret"];
        static string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
        static string authurl = string.Format(aadInstance, tenantID);
        public  async static Task<string> getTokenAsync()
        {
            string token = "";
            AuthenticationResult result;
            
            IConfidentialClientApplication app;
            app = ConfidentialClientApplicationBuilder.Create(clientId)
           .WithClientSecret(clientSecret)
           .WithAuthority(authurl)
           .Build();

            try
            {
                result = await app.AcquireTokenForClient(scopes)
                                 .ExecuteAsync();
                token = result.AccessToken;
            }
            catch (MsalUiRequiredException ex)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
            }
            return token;
        }


    }
}