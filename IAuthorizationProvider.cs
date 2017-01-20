using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace OAuthLogin
{
    /// <summary>
    /// 授权提供程序接口。
    /// </summary>
    public interface IAuthorizationProvider
    {
        /// <summary>
        /// 生成授权Url。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="state">状态参数。</param>
        /// <param name="view">授权页面的类型，如淘宝将授权页面分为web、tmall和wap三种类型。</param>
        string GenerateUrl(Application application, string state = "", string view = "");

        /// <summary>
        /// 接受回调请求后向平台换取Token。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="callbackRequest">回调请求。</param>
        AuthorizationResult GetToken(Application application, HttpRequest callbackRequest);

        /// <summary>
        /// 异步向平台换取Token。
        /// </summary>
        /// <param name="application">平台应用</param>
        /// <param name="callbackRequest">回调请求。</param>
        Task<AuthorizationResult> GetTokenAsync(Application application, HttpRequest callbackRequest);
    }//end IAuthorizationProvider

}//end namespace OAuthLogin