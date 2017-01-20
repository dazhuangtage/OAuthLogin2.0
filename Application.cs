using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace OAuthLogin
{
    /// <summary>
    /// 描述平台中的应用，存储该应用的基本接口信息。
    /// 应用是平台用来管理接口调用权限的机制。业务系统方要访问平台接口必须先申请一个应用，经平台方审核通过后才具有调用相应接口的权限。
    /// </summary>
    public class Application
    {

        /// <summary>
        /// 应用在其所属平台的唯一标识
        /// </summary>
        private string _appKey;

        /// <summary>
        /// 获取或设置应用在其所属平台的唯一标识。
        /// </summary>
        public string AppKey
        {
            get
            {
                return _appKey;
            }
            set
            {
                _appKey = value;
            }
        }


        /// <summary>
        /// 应用的密钥
        /// </summary>
        private string _secret;

        /// <summary>
        /// 获取或设置应用的密钥。
        /// </summary>
        public string Secret
        {
            get
            {
                return _secret;
            }
            set
            {
                _secret = value;
            }
        }

        /// <summary>
        /// 应用的授权回调Url
        /// </summary>
        private string _redirectUrl;

        /// <summary>
        /// 获取或设置应用的授权回调Url。
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                return _redirectUrl;
            }
            set
            {
                _redirectUrl = value;
            }
        }

        /// <summary>
        /// 应用所属的平台
        /// </summary>
        private Platform _platform;

        /// <summary>
        /// 获取应用所属的平台。
        /// </summary>
        public Platform Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// 授权提供程序实例
        /// </summary>
        private IAuthorizationProvider _provider;

        /// <summary>
        /// 授权提供程序实例
        /// </summary>
        public IAuthorizationProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        /// <summary>
        /// 创建Application实例。
        /// </summary>
        /// <param name="platform">应用所属的平台。</param>
        /// <param name="provider">授权提供程序实例。</param>
        public Application(Platform platform, IAuthorizationProvider provider) : base()
        {
            this._platform = platform;
            this._provider = provider;
        }

        /// <summary>
        /// 生成授权Url。
        /// </summary>
        /// <param name="state">状态参数。</param>
        /// <param name="view">授权页面的类型，如淘宝将授权页面分为web、tmall和wap三种类型。</param>
        public string GenerateAuthorizationUrl(string state, string view = "")
        {
            return Provider.GenerateUrl(this, state, view);
        }

        /// <summary>
        /// 接受回调请求后向平台换取Token。
        /// </summary>
        /// <param name="callbackRequest">回调请求。</param>
        public AuthorizationResult GetToken(HttpRequest callbackRequest)
        {
            return Provider.GetToken(this, callbackRequest);
        }

        /// <summary>
        /// 异步向平台换取Token。
        /// </summary>
        /// <param name="callbackRequest">回调请求。</param>
        public async Task<AuthorizationResult> GetTokenAsync(HttpRequest callbackRequest)
        {
            return await Provider.GetTokenAsync(this, callbackRequest);
        }

    }//end Application

}//end namespace OAuthLogin