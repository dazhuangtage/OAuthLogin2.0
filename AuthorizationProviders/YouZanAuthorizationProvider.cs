using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace OAuthLogin
{
    /// <summary>
    /// 适用于有赞的授权提供程序。
    /// </summary>
    public class YouZanAuthorizationProvider : AuthorizationProvider, IAuthorizationProvider
    {

        /// <summary>
        /// 生成授权Url。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="state">用于保持请求和回调的状态，在回调时，会回传该参数。开发者可以用这个参数验证请求有效性，也可以记录用户请求授权页前的位置。可防止CSRF攻击{示例值：teststate}</param>
        /// <param name="view">未启用(授权页面的类型，如淘宝将授权页面分为web、tmall和wap三种类型)</param>
        public override string GenerateUrl(Application application, string state = "", string view = "")
        {
            string app_id = application.AppKey;//[必须]有赞的AppId
            string response_type = "code";
            string templet = application.Platform.AuthorizationUrl + "?client_id={0}&response_type={1}&state={2}";
            StringBuilder authorizationCode = new StringBuilder();
            authorizationCode.AppendFormat(templet, app_id, response_type, state);
            return authorizationCode.ToString();
        }

        /// <summary>
        /// 接受回调请求后向平台换取Token。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="callbackRequest">回调请求。</param>
        public override async Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest)
        {
            try
            {
                string tokenUrl = application.Platform.TokenUrl;
                string code = callbackRequest["code"];
                var clientid = application.AppKey;//分配的调用Oauth的应用端ID(client_id)"a20ed852cb548fac9b"
                var clientsecret = application.Secret;//分配的调用Oauth的应用端Secret(client_secret)"c84925ce527c3bf452cd086dfc135b91"
                var granttype = "authorization_code";//授与方式[固定为"authorization_code"](grant_type)
                string redirectUrl = application.RedirectUrl;

                string toktemplet = "client_id={0}&client_secret={1}&grant_type={2}&code={3}&redirect_uri={4}";

                string tokenPar = string.Format(toktemplet, clientid, clientsecret, granttype, code, redirectUrl);

                var jsonRsult =await HttpHelp.GetStrAsync(tokenUrl + "?" + tokenPar);
                YouZanTokenResult tokenResult = new YouZanTokenResult();
                tokenResult = await Task.Factory.StartNew(()=> { return JsonConvert.DeserializeObject<YouZanTokenResult>(jsonRsult); });
                AuthorizationResult result = new AuthorizationResult
                {
                    ExpireAt = DateTime.Now.AddMinutes(-3).AddSeconds(tokenResult.expires_in),
                    RefreshExpireAt = DateTime.Now.AddMinutes(-3).AddSeconds(tokenResult.expires_in),
                    RefreshToken = tokenResult.refresh_token,
                    Token = tokenResult.access_token,
                    TokenType = tokenResult.token_type,
                    OpenId = string.Empty,
                    UserName = ""
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private class YouZanTokenResult
        {
            /// <summary>
            /// 必须
            /// 可用于获取资源的 AccessToken
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 必须
            /// AccessToken 的有效时长，单位：秒
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 非必须
            /// 令牌类型
            /// </summary>
            public string token_type { get; set; }
            /// <summary>
            /// 非必须
            /// AccessToken 最终的访问范围
            /// </summary>
            public string scope { get; set; }
            /// <summary>
            /// 非必须
            /// 用于刷新 AccessToken 的 RefreshToken，不是所有的应用都有该参数（过期时间：28 天）
            /// </summary>
            public string refresh_token { get; set; }

        }
    }
}