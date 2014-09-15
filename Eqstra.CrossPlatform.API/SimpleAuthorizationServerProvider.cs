using Eqstra.DataProvider.AX.Repositories;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Eqstra.CrossPlatform.API
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
       async public override System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
             context.Validated();
        }
        async public override System.Threading.Tasks.Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            using (SSRepository ssRepo = new SSRepository())
            {
                var userInfo = await ssRepo.ValidateUserAsync(null, context.UserName, context.Password);
                if (userInfo == null)
                {
                    context.SetError("invalid_grant", "The username or password is incorrect.");
                    return;
                }
                
                
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            context.Validated(identity);     
          
            
        }
    }
}
