using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Graph;
using Microsoft.Graph.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AuthWebAppDotNet.Helpers
{
    public class RolesProvider
    {
        static string graphInstance = "https://graph.microsoft.com/v1.0/users/{0}/getGroupMembers";
        static string getRoleUrl = "https://graph.microsoft.com/v1.0/users/{0}/memberOf";

        //getMemberGroups, actualy thisi is post method
        public static async Task<string> GetUserGroupById(string userid, string accessToken)
        {
            string content = "";
            string graphurl =string.Format(graphInstance, userid);
            HttpClient httpClient = new HttpClient();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            
            HttpContent _body = new StringContent("{\"SecurityEnabledOnly\":\"true\"}",Encoding.UTF8,"application/json");
           

            HttpResponseMessage response = await httpClient.PostAsync(graphurl,_body);
             content =  response.Content.ReadAsStringAsync().Result;
            
           var contentresponse =  JObject.Parse(content.ToString());
            return contentresponse.ToString();

        }

         
        public static async Task<IList<string>> GetRoleByUserId(string userid, string accessToken)
        {
            string content = "";
            IList<string> roles = new List<string>();
            string graphurl = string.Format(getRoleUrl, userid);
            HttpClient httpClient = new HttpClient();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);           

            HttpResponseMessage response = await httpClient.GetAsync(graphurl);
            content = response.Content.ReadAsStringAsync().Result;

            var contentresponse = JObject.Parse(content.ToString());

            if (contentresponse["value"].First != null)
            {

                var displayNames = (JArray)contentresponse["value"];
                foreach (var items in displayNames.Select(x => x["displayName"]))
                {
                    roles.Add(items.ToString());
                }
                
                return roles;
            }
            else
                return roles;
        }
       
    }

   
}
