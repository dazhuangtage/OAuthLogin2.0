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
    /// 适用于京东的授权提供程序。OAuth2.0
    /// </summary>
    public class JdAuthorizationProvider : AuthorizationProvider, IAuthorizationProvider
    {



        /// <summary>
        /// 生成授权Url。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="state">状态参数(状态参数，由ISV自定义，颁发授权后会原封不动返回)</param>
        /// <param name="view">移动端授权，该值固定为wap；非移动端授权，无需传值</param>
        public override string GenerateUrl(Application application, string state = "", string view = "")
        {
            string responseType = "code"; //此流程下，该值固定为code
            string clientId = application.AppKey;     //即创建应用时的Appkey（从JOS控制台->管理应用中获取）
            string redirectUri = application.RedirectUrl;  //即应用的回调地址，必须与创建应用时所填回调页面url一致

            StringBuilder authorizationCode = new StringBuilder();
            authorizationCode.AppendFormat(
                "https://oauth.jd.com/oauth/authorize?response_type={0}&client_id={1}&redirect_uri={2}", responseType,
                clientId, redirectUri);
            if (!string.IsNullOrEmpty(state))
            {
                authorizationCode.AppendFormat("&state={0}", state);
            }
            if (view.ToLower().Equals("wap"))
            {
                authorizationCode.AppendFormat("&view={0}", view);
            }

            return authorizationCode.ToString();
        }

        /// <summary>
        /// 接受回调请求后向平台换取Token。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="callbackRequest">回调请求。</param>
        public override async Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest)
        {
            var error = callbackRequest["error"];
            if (null != error) return null;
            var grantType = "authorization_code";//授权类型，此流程下，该值固定为authorization_code
            var code = callbackRequest["code"];//授权请求返回的授权码
            var redirectUri = application.RedirectUrl;//应用的回调地址，必须与创建应用时所填回调页面url一致
            var clientId = application.AppKey;//即创建应用时的Appkey（从JOS控制台->管理应用中获取）
            var clientSecret = application.Secret;//即创建应用时的Appsecret（从JOS控制台->管理应用中获取）
            var clientState = "";//状态参数，由ISV自定义，颁发授权后会原封不动返回
            string tokenurl = "https://open.koudaitong.com/oauth/token";//获取token的地址
            StringBuilder tokenurlpar = new StringBuilder();
            tokenurlpar.AppendFormat("grant_type={0}&code={1}&redirect_uri={2}&client_id={3}&client_secret={4}", grantType, code, redirectUri, clientId, clientSecret);
            if (string.IsNullOrWhiteSpace(clientState)) tokenurlpar.AppendFormat("&state={0}", clientState);
            var jsonResult = await HttpHelp.GetStrAsync(tokenurl + "/" + tokenurlpar.ToString());
            JdTokenResult tokenResult = tokenResult = await Task.Factory.StartNew(() => { return JsonConvert.DeserializeObject<JdTokenResult>(jsonResult); });
            if (string.IsNullOrEmpty(tokenResult.access_token)) return null;
            AuthorizationResult result = new AuthorizationResult
            {
                ExpireAt = new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(tokenResult.time) + double.Parse(tokenResult.expires_in)),
                RefreshExpireAt = new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(tokenResult.time) + double.Parse(tokenResult.expires_in)),
                RefreshToken = tokenResult.refesh_token,
                Token = tokenResult.access_token,
                TokenType = tokenResult.token_type,
                OpenId = tokenResult.uid,
                UserName = tokenResult.user_nick
            };
            return result;
        }

        private struct JdTokenResult
        {
            /// <summary>
            /// 授权token
            /// </summary>
            public string access_token { get; set; }
            public string code { get; set; }
            /// <summary>
            /// 失效时间（从当前时间算起，单位：秒）
            /// </summary>
            public string expires_in { get; set; }
            public string refesh_token { get; set; }
            public string scope { get; set; }
            /// <summary>
            /// 授权的时间点（UNIX时间戳，单位：毫秒）
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// token类型（暂无意义）
            /// </summary>
            public string token_type { get; set; }
            /// <summary>
            /// 授权用户对应的京东ID
            /// </summary>
            public string uid { get; set; }
            /// <summary>
            /// 授权用户对应的京东昵称
            /// </summary>
            public string user_nick { get; set; }
        }

    }//end JdAuthorizationProvider
}//end namespace OAuthLogin