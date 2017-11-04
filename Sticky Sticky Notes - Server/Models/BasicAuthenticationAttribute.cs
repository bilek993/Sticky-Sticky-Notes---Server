using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace Sticky_Sticky_Notes___Server.Models
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string token = actionContext.Request.Headers.Authorization.Parameter;
                string[] userData = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':'); // username:password

                if (userData.Length != 2)
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                string username = userData[0];
                string password = userData[1];

                if (UsersHelper.CheckUserCredentials(username, password))
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                else
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}