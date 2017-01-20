using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OAuthLogin
{
    public class QQAuthorizationProvider : AuthorizationProvider, IAuthorizationProvider
    {
        private const string GetOpenIdApiUrl = "https://graph.qq.com/oauth2.0/me?access_token=";
        public override string GenerateUrl(Application application, string state = "", string view = "")
        {
            return $"{application.Platform.AuthorizationUrl}?response_type=code&client_id={application.AppKey}&redirect_uri={application.RedirectUrl}&state={state}";
        }

        public override async Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest)
        {
            if (callbackRequest == null)
                throw new ArgumentNullException(nameof(callbackRequest));
            var code = callbackRequest.QueryString["code"];
            var grant_type = "authorization_code";
            var client_id = application.AppKey;
            var client_secret = application.Secret;
            var redirect_uri = application.RedirectUrl;
            StringBuilder requestPar = new StringBuilder();
            requestPar.Append($"grant_type={grant_type}&");
            requestPar.Append($"client_id={client_id}&");
            requestPar.Append($"client_secret={client_secret}&");
            requestPar.Append($"redirect_uri={redirect_uri}&");
            requestPar.Append($"code={code}");
            var result = await HttpHelp.GetStrAsync(GenerateApiUrl(application, requestPar.ToString())).ConfigureAwait(false);
            ValidateResult(result);
            QQAuthorizationTokenResult tokenResult = new QQAuthorizationTokenResult();
            await Task.Factory.StartNew(() => { tokenResult = AnalyParameter(result); }).ConfigureAwait(false);
            AuthorizationResult authorizationResult = new AuthorizationResult
            {
                ExpireAt = DateTime.Now.AddMinutes(-3).AddSeconds(tokenResult.expires_in),
                RefreshExpireAt = DateTime.Now.AddMinutes(-3).AddSeconds(tokenResult.expires_in),
                Token = tokenResult.access_token,
                OpenId =await GetOpenId(tokenResult.access_token).ConfigureAwait(false)
            };
            return authorizationResult;
        }

        private async Task<string> GetOpenId(string token)
        {
            var url = GetOpenIdApiUrl + token;
            var result = await HttpHelp.GetStrAsync(url).ConfigureAwait(false);
            ValidateResult(result);
            result = result.Replace("callback(", "").Replace(");", "");
            var jsonResult = (JObject)JsonConvert.DeserializeObject(result);
            return jsonResult["openid"].ToString();
        }

        public void ValidateResult(string result)
        {
            if (result.IndexOf("error") > -1) ThrowException(result);
        }

        private string GenerateApiUrl(Application application, string parameter)
        {
            return application.Platform.TokenUrl + "?" + parameter.TrimEnd('&');
        }

        [Serializable]
        private struct QQAuthorizationTokenResult
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }

        private QQAuthorizationTokenResult AnalyParameter(string urlResult)
        {
            object qAuthorizationTokenResult = new QQAuthorizationTokenResult();
            var urlResults = urlResult.Split('&');
            Parallel.For(0, urlResults.Length, (i) =>
            {
                var attbuiter = urlResults[i].Split('=')[0];
                var value = urlResults[i].Split('=')[1];
                var propertyInfo = qAuthorizationTokenResult.GetType().GetProperty(attbuiter, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(qAuthorizationTokenResult, Convert.ChangeType(value, propertyInfo.PropertyType));
                }
            });
            return (QQAuthorizationTokenResult)qAuthorizationTokenResult;
        }

        private void ThrowException(string resut)
        {
            var replaceResult = resut.Replace("callback(", "").Replace(");", "");
            var jobj = (JObject)JsonConvert.DeserializeObject(replaceResult);
            var code = jobj["error"];
            var desc = jobj["error_description"];
            throw EtpExceptionBuilderContainer.Current.GetBuilder(EtpName.QQ).Create(code.ToString(), desc.ToString());

        }
    }
}
