using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;


//using OAuthLogin.ExtendConfiguration;

namespace OAuthLogin
{
    /// <summary>
    /// 表示外部交易平台，存储该平台的基本接口信息。
    /// </summary>
    public class Platform
    {

        /// <summary>
        /// 授权Url
        /// </summary>
        private string _authorizationUrl;

        /// <summary>
        /// 换取令牌的Url
        /// </summary>
        private string _tokenUrl;

        /// <summary>
        /// 接口调用Url
        /// </summary>
        private string _apiUrl;

        /// <summary>
        /// 平台的名称
        /// </summary>
        private EtpName _name;

        /// <summary>
        /// 包含的应用
        /// </summary>
		private Application[] _applications;

        private Platform()
        {

        }

        ~Platform()
        {

        }

        /// <summary>
        /// 获取或设置平台的授权Url。
        /// </summary>
        public string AuthorizationUrl
        {
            get
            {
                return _authorizationUrl;
            }
            set
            {
                _authorizationUrl = value;
            }
        }

        /// <summary>
        /// 获取或设置平台的令牌换取Url。
        /// </summary>
        public string TokenUrl
        {
            get
            {
                return _tokenUrl;
            }
            set
            {
                _tokenUrl = value;
            }
        }

        /// <summary>
        /// 获取或设置平台的接口访问Url。
        /// </summary>
        public string ApiUrl
        {
            get
            {
                return _apiUrl;
            }
            set
            {
                _apiUrl = value;
            }
        }

        /// <summary>
        /// 获取或设置平台的名称。
        /// </summary>
        public EtpName Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 获取当前平台下的所有应用。
        /// </summary>
        public Application[] Applications
        {
            get { return _applications; }
            set { _applications = value; }
        }

        /// <summary>
        /// 根据名称查找Etp。
        /// </summary>
        /// <param name="name">平台的名称。</param>
        public static Platform Find(EtpName name)
        {
            if (string.IsNullOrWhiteSpace(name.Name))
            {
                throw new Exception("未指定平台名");
            }
            Platform platform = new Platform();
            //配置文件中查找平台属性填充(注意填充平台下包含的应用)
            OAuthLoginConfig oAuthLoginConfig;
            try
            {
                oAuthLoginConfig = (OAuthLoginConfig)ConfigurationManager.GetSection("OAuthLogin");
            }
            catch
            {
                throw new Exception("配置错误");
            }
            var etpSection = oAuthLoginConfig.Etps[name.Name.ToLower().Trim()];
            if (etpSection == null)
            {
                throw new Exception("平台未配置");
            }
            //填充平台数据
            platform.Name = name;
            platform.ApiUrl = etpSection.ApiUrl;
            platform.AuthorizationUrl = etpSection.AuthorizationUrl;
            platform.TokenUrl = etpSection.TokenUrl;
            platform.Applications = new Application[etpSection.Apps.Count];

            //授权提供程序
            IAuthorizationProvider provider = AuthorizationProviderContainer.Current.GetProvider(name);
            for (int i = 0; i < etpSection.Apps.Count; i++)
            {
                //填充平台下应用数据
                Application appliction = new Application(platform, provider);
                appliction.AppKey = etpSection.Apps[i].Appkey;
                appliction.RedirectUrl = etpSection.Apps[i].RedirectUrl;
                appliction.Secret = etpSection.Apps[i].Secret;
                platform.Applications[i] = appliction;
            }
            //返回平台
            return platform;
        }

        /// <summary>
        /// 获取当前平台下具有指定标识的应用。
        /// </summary>
        /// <param name="key">应用在其所属平台内的唯一标识。</param>
        public Application GetApplication(string key)
        {
            Application application = null;
            //遍历当前平台下的应用
            foreach (Application app in Applications)
            {
                if (app.AppKey == key)
                {
                    return app;
                }
            }
            return application;
        }

    }//end Platform

}//end namespace OAuthLogin