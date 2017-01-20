using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace OAuthLogin
{
    /// <summary>
    /// 一个控制反转容器，用以说明不同平台的Application应该适用哪种类型的授权提供程序。
    /// 本容器负责从配置文件读取依赖关系，采用反射方式创建提供程序实例，并提供缓存机制。
    /// </summary>
    public class AuthorizationProviderContainer
    {

        /// <summary>
        /// 提供程序缓存集合
        /// </summary>
        private Dictionary<string, IAuthorizationProvider> dicCatheProvider = null;

        /// <summary>
        /// AuthorizationProviderContainer实例
        /// </summary>
        private static AuthorizationProviderContainer authorizationProviderContainer;

        /// <summary>
        /// 创建AuthorizationProviderContainer实例。
        /// </summary>
        private AuthorizationProviderContainer()
        {

        }
        static object lockO = new object();
        /// <summary>
        /// 获取对应于指定平台的授权提供程序
        /// </summary>
        /// <param name="etpName">平台名称。</param>
        public IAuthorizationProvider GetProvider(EtpName etpName)
        {
            IAuthorizationProvider provider = null;
            if (dicCatheProvider.ContainsKey(etpName.Name.ToLower().Trim()))
            {
                //从缓存中取
                provider = dicCatheProvider[etpName.Name.ToLower().Trim()];
            }
            else
            {
                lock (lockO)
                {
                    if (!dicCatheProvider.ContainsKey(etpName.Name.ToLower().Trim()))
                    {
                        //从配置文件创建
                        OAuthLoginConfig freepEtp = (OAuthLoginConfig)ConfigurationManager.GetSection("OAuthLogin");

                        var authorizationProvider = freepEtp.AuthorizationProvider[etpName.Name.ToLower().Trim()];

                        Assembly assembly = Assembly.Load(authorizationProvider.Assembly);
                        //反射创建提供程序实例
                        object obj = assembly.CreateInstance(authorizationProvider.Type);
                        //添加缓存
                        authorizationProviderContainer.dicCatheProvider.Add(authorizationProvider.Etp, (IAuthorizationProvider)obj);

                        provider = (IAuthorizationProvider)obj;
                    }
                }
            }
            return provider;
        }
        static object lockObj = new object();
        /// <summary>
        /// 获取当前应用程序域中的AuthorizationProviderContainer实例。
        /// 本属性应当确保当前应用程序域中有且仅有一个AuthorizationProviderContainer实例。
        /// </summary>
        public static AuthorizationProviderContainer Current
        {
            get
            {
                if (authorizationProviderContainer == null)
                {
                    lock (lockObj)
                    {
                        if (authorizationProviderContainer == null)
                        {
                            authorizationProviderContainer = new AuthorizationProviderContainer();
                            //初始化缓存集合
                            authorizationProviderContainer.dicCatheProvider = new Dictionary<string, IAuthorizationProvider>();
                            //反射添加缓存
                            OAuthLoginConfig freepEtp = (OAuthLoginConfig)ConfigurationManager.GetSection("OAuthLogin");
                            var Providers = freepEtp.AuthorizationProvider;
                            //遍历不同平台的程序集
                            for (int i = 0; i < Providers.Count; i++)
                            {
                                Console.WriteLine(i.ToString());
                                Assembly assembly = Assembly.Load(Providers[i].Assembly);
                                //反射创建提供程序实例
                                object obj = assembly.CreateInstance(Providers[i].Type);
                                if (!authorizationProviderContainer.dicCatheProvider.ContainsKey(Providers[i].Etp))
                                {
                                    authorizationProviderContainer.dicCatheProvider[Providers[i].Etp] = (IAuthorizationProvider)obj;
                                }
                            }
                        }
                    }
                }
                return authorizationProviderContainer;
            }
        }

    }//end AuthorizationProviderContainer

}//end namespace OAuthLogin