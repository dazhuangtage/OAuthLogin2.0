using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OAuthLogin
{
    public abstract class AuthorizationProvider : IAuthorizationProvider
    {
        public abstract string GenerateUrl(Application application, string state = "", string view = "");

        public virtual AuthorizationResult GetToken(Application application, HttpRequest callbackRequest)
        {
            try
            {
                AuthorizationResult result = new AuthorizationResult();
                var tokenAysnResult = GetTokenAsync(application, callbackRequest);
                tokenAysnResult.Wait();
                return tokenAysnResult.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public abstract Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest);
    }
}
