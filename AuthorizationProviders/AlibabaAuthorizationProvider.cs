using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace OAuthLogin
{
    /// <summary>
    /// 适用于阿里巴巴的授权提供程序。
    /// </summary>
    public class AlibabaAuthorizationProvider : AuthorizationProvider, IAuthorizationProvider
    {


        /// <summary>
        /// 生成授权Url。
        /// </summary>
        /// <param name="application">应用信息。</param>
        /// <param name="state">状态参数。</param>
        /// <param name="view">授权页面的类型，如淘宝将授权页面分为web、tmall和wap三种类型。</param>
        public override string GenerateUrl(Application application, string state = "", string view = "")
        {

            #region 请求参数
            //app注册时，分配给app的唯一标示，又称appKey
            string client_id = application.AppKey;
            //site参数标识当前授权的站点，直接填写china
            string site = "china";
            //app的入口地址，授权临时令牌会以queryString的形式跟在该url后返回
            string redirect_uri = application.RedirectUrl;

            //参数对集合
            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(state))
            {
                paramDic.Add("state", state);
            }
            paramDic.Add("client_id", client_id);
            paramDic.Add("site", site);
            paramDic.Add("redirect_uri", redirect_uri);
            //签名
            string _aop_signature = sign(paramDic, application.Secret);

            #endregion

            //拼接URL
            string url = string.Format("{0}?client_id={1}&site={2}&redirect_uri={3}&state={4}&_aop_signature={5}",
                application.Platform.AuthorizationUrl, client_id, site, redirect_uri, string.IsNullOrWhiteSpace(state) ? "" : state, _aop_signature);

            //返回URL
            return url;
        }

        /// <summary>
        /// 接受回调请求后向平台换取Token。
        /// </summary>
        /// <param name="application">应用信息。</param>
        /// <param name="callbackRequest">回调请求。</param>
        public override async Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest)
        {

            #region 获取Token请求参数

            //从请求报文中获取签名
            string code = callbackRequest.Params["Code"];
            //授权类型，使用authorization_code即可
            string grant_type = "authorization_code";
            //是否需要返回refresh_token，如果返回了refresh_token，原来获取的refresh_token也不会失效，除非超过半年有效期
            string need_refresh_token = "true";
            //app唯一标识，即appKey
            string client_id = application.AppKey;
            //app密钥
            string client_secret = application.Secret;
            //app入口地址
            string redirect_uri = application.RedirectUrl;

            #endregion

            #region 生成请求URL
            string url = application.Platform.TokenUrl.Replace("YOUR_APPKEY", application.AppKey);
            string para = string.Format("grant_type={0}&need_refresh_token={1}&client_id= {2}&client_secret= {3}&redirect_uri={4}&code={5}",
                grant_type, need_refresh_token, client_id, client_secret, redirect_uri, code);

            #endregion

            #region 发送请求及返回结果

            try
            {
                //发送请求报文
                string json = await HttpHelp.GetStrAsync(url + "?" + para);
                //判断是否返回异常
                if (json.Contains("error"))
                {
                    AlibabaException alibabaException = await Task.Factory.StartNew(() => { return JsonConvert.DeserializeObject<AlibabaException>(json); });
                    //抛出异常
                    if (string.IsNullOrWhiteSpace(alibabaException.Error))
                    {
                        throw EtpException.Create(application.Platform.Name, alibabaException.Error_code.ToString(), alibabaException.Error_message, "", "");
                    }
                    else
                    {
                        throw EtpException.Create(application.Platform.Name, alibabaException.Error, alibabaException.Error_description, "", "");
                    }
                }
                else
                {
                    //把返回的json字符串反序列化
                    AlibabaJson alibabaJson = await Task.Factory.StartNew(() => { return JsonConvert.DeserializeObject<AlibabaJson>(json); });
                    //填充AuthorizationResult数据
                    return new AuthorizationResult()
                    {
                        Token = alibabaJson.Access_token,
                        OpenId =alibabaJson.AliId,
                        RefreshToken = alibabaJson.Refresh_token,
                        UserName = alibabaJson.MemberId,
                        ExpireAt = DateTime.Now.AddMinutes(-1).AddSeconds(alibabaJson.Expires_in),
                        RefreshExpireAt = DateTime.Parse(alibabaJson.Refresh_token_timeout.Substring(0, 4) + "-" +
                        alibabaJson.Refresh_token_timeout.Substring(4, 2) + "-" +
                        alibabaJson.Refresh_token_timeout.Substring(6, 2) + " " +
                        alibabaJson.Refresh_token_timeout.Substring(8, 2) + ":" +
                        alibabaJson.Refresh_token_timeout.Substring(10, 2) + ":" +
                        alibabaJson.Refresh_token_timeout.Substring(12, 2))
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        #region 辅助方法及结构
        /// <summary>
        /// 阿里巴巴签名生成
        /// </summary>
        /// <param name="paramDic">请求参数，即queryString + request body 中的所有参数</param>
        /// <param name="appSecret">app密钥，与请求参数中client_id表示的appKey对应</param>
        /// <returns></returns>
        private string sign(Dictionary<string, string> paramDic, string appSecret)
        {
            byte[] signatureKey = Encoding.UTF8.GetBytes(appSecret);
            //第一步：拼装key+value
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                list.Add(kv.Key + kv.Value);
            }
            //第二步：排序
            list.Sort();
            //第三步：拼装排序后的各个字符串
            string tmp = "";
            foreach (string kvstr in list)
            {
                tmp = tmp + kvstr;
            }
            //第四步：将拼装后的字符串和app密钥一起计算签名
            //HMAC-SHA1
            HMACSHA1 hmacsha1 = new HMACSHA1(signatureKey);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            byte[] hash = hmacsha1.Hash;
            //TO HEX
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        }


        /// <summary>
        /// 阿里巴巴json对象
        /// </summary>
        private struct AlibabaJson
        {
            /// <summary>
            /// 阿里巴巴集团统一的id
            /// </summary>
            public string AliId { get; set; }

            /// <summary>
            /// 登录id
            /// </summary>
            public string Resource_owner { get; set; }

            /// <summary>
            /// 会员接口id
            /// </summary>
            public string MemberId { get; set; }

            public int Expires_in { get; set; }

            public string Refresh_token { get; set; }

            /// <summary>
            /// refreshToken的过期时间
            /// </summary>
            public string Refresh_token_timeout { get; set; }

            public string Access_token { get; set; }
        }

        /// <summary>
        /// 阿里巴巴异常对象
        /// </summary>
        private struct AlibabaException
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int Error_code { get; set; }
            /// <summary>
            /// 错误消息
            /// </summary>
            public string Error_message { get; set; }

            /// <summary>
            /// 异常对象
            /// </summary>
            public string Exception { get; set; }

            /// <summary>
            /// 错误
            /// </summary>
            public string Error { get; set; }

            /// <summary>
            /// 错误描述
            /// </summary>
            public string Error_description { get; set; }
        }

        #endregion

    }//end AlibabaAuthorizationProvider

}//end namespace OAuthLogin