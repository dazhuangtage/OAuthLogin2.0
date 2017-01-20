using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace OAuthLogin
{
    /// <summary>
    /// 适用于美丽说的授权提供程序。
    /// </summary>
    public class MeiliShuoAuthorizationProvider : AuthorizationProvider, IAuthorizationProvider
    {


        /// <summary>
        /// 生成授权Url。
        /// </summary>
        /// <param name="application">平台应用。</param>
        /// <param name="state">状态参数。</param>
        /// <param name="view">授权页面的类型，如淘宝将授权页面分为web、tmall和wap三种类型。</param>
        public override string GenerateUrl(Application application, string state = "", string view = "")
        {

            #region URL参数
            //响应类型
            string response_type = "token";
            //应用的  key
            string client_id = application.AppKey;
            //回调URL
            string redirect_uri = application.RedirectUrl;

            #endregion

            //拼接URL
            string url = string.Format("{0}?response_type={1}&app_key={2}&redirect_uri={3}{4}",
                application.Platform.AuthorizationUrl, response_type, client_id, redirect_uri, string.IsNullOrWhiteSpace(state) ? "" : "&state=" + state);

            //返回URL
            return url;
        }

        /// <summary>
        /// 接受回调请求后向平台换取Token。
        /// </summary>
        /// <param name="application">平台应用。</param>
        /// <param name="callbackRequest">回调请求。</param>
        public override async Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest)
        {


            #region 请求参数
            //授权码
            string code = callbackRequest.QueryString["code"];
            //应用app  key
            string client_id = application.AppKey;
            //应用密匙
            string client_secret = application.Secret;
            //换取方式
            string grant_type = "authorization_code";
            //回调请求
            string redirect_uri = application.RedirectUrl;

            #endregion

            #region 生成请求URL
            //请求URL
            string url = application.Platform.TokenUrl;
            //请求参数拼接
            string para = string.Format("code={0}&app_key={1}&app_secret={2}&grant_type={3}&redirect_uri={4}",
                code, client_id, client_secret, grant_type, redirect_uri);

            #endregion

            #region 发送请求及处理结果

            //返回json字符串结果
            string json = await HttpHelp.GetStrAsync(url + "?" + para);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            if (json.Contains("data"))
            {
                //反序列化json字符串
                MeiLiShuoData meiLiShuoData = await Task.Factory.StartNew(() => { return Newtonsoft.Json.JsonConvert.DeserializeObject<MeiLiShuoData>(json); });
                MeiLiShuoJson meiLiShuoJson = meiLiShuoData.data;
                //填充数据返回
                return new AuthorizationResult()
                {
                    OpenId = meiLiShuoJson.Meilishuo_user_id,
                    Token = meiLiShuoJson.Access_token,
                    RefreshToken = meiLiShuoJson.Refresh_token,
                    TokenType = meiLiShuoJson.Token_type,
                    ExpireAt = DateTime.Now.AddMinutes(-1).AddSeconds(36000),
                    RefreshExpireAt = DateTime.FromFileTimeUtc(meiLiShuoJson.Re_expires_in),
                    UserName = meiLiShuoData.data.Meilishuo_shop_nick
                };

            }
            else//异常处理
            {
                //抛异常
                MeiLiShuoExecption ex = jss.Deserialize<MeiLiShuoExecption>(json);
                throw EtpException.Create(application.Platform.Name, ex.Error_code, ex.Message, "", "");
            }
            #endregion

        }

        private struct MeiLiShuoData
        {
            public MeiLiShuoJson data { get; set; }
            public int error_code { get; set; }
        }

        /// <summary>
        /// 美丽说返回正确结果结构
        /// </summary>
        private struct MeiLiShuoJson
        {
            /// <summary>
            /// Access token
            /// </summary>
            public string Access_token { get; set; }

            /// <summary>
            /// Access token的类型目前只支持bearer
            /// </summary>
            public string Token_type { get; set; }

            /// <summary>
            /// Access token过期时间
            /// </summary>
            public int Expires_in { get; set; }

            /// <summary>
            /// Refresh token
            /// </summary>
            public string Refresh_token { get; set; }

            /// <summary>
            /// Refresh token过期时间
            /// </summary>
            public int Re_expires_in { get; set; }

            /// <summary>
            /// 美丽说帐号对应id
            /// </summary>
            public string Meilishuo_user_id { get; set; }

            public string Meilishuo_shop_id { get; set; }

            public string Meilishuo_shop_nick { get; set; }
        }

        private struct MeiLiShuoExecption
        {
            public string Error_code { get; set; }
            public string Code { get; set; }
            public string Message { get; set; }
        }

    }//end MeiliShuoAuthorizationProvider

}//end namespace OAuthLogin