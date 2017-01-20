using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace OAuthLogin
{
    /// <summary>
    /// 一个控制反转容器，用以说明构造EtpException实例时应该选择哪种类型的构造器。
    /// 本容器负责从配置文件读取依赖关系，采用反射方式创建构造器实例，并提供缓存机制。
    /// </summary>
    public class EtpExceptionBuilderContainer
    {

        /// <summary>
        /// EtpException构造器缓存集合
        /// </summary>
        private Dictionary<string, IEtpExceptionBuilder> dicCatheBuilder = null;

        /// <summary>
        ///  创建EtpExceptionBuilderContainer实例。
        /// </summary>
        private static EtpExceptionBuilderContainer etpExceptionBuilderContainer;

        /// <summary>
        /// 创建EtpExceptionBuilderContainer实例。
        /// </summary>
        private EtpExceptionBuilderContainer()
        {

        }

        /// <summary>
        /// 获取适用于指定平台的EtpException构造器。
        /// </summary>
        /// <param name="etpName"></param>
        public IEtpExceptionBuilder GetBuilder(EtpName etpName)
        {

            //有则从缓存中取
            if (dicCatheBuilder.ContainsKey(etpName.Name.ToLower().Trim()))
            {
                return dicCatheBuilder[etpName.Name.ToLower().Trim()];
            }
            else
            {
                //反射创建
                OAuthLoginConfig authLoginConfig = (OAuthLoginConfig)ConfigurationManager.GetSection("OAuthLogin");

                var builder = authLoginConfig.EtpExceptionBuilder[etpName.Name.ToLower().Trim()];
                Assembly assembly = Assembly.Load(builder.Assembly);

                //反射创建构造器实例
                object obj = assembly.CreateInstance(builder.Type);
                //添加缓存
                etpExceptionBuilderContainer.dicCatheBuilder.Add(builder.Etp, (IEtpExceptionBuilder)obj);
                return (IEtpExceptionBuilder)obj;
            }
        }

        static object lockObj = new object();
        /// <summary>
        /// 获取当前应用程序域中的EtpExceptionBuilderContainer实例。
        /// 该属性应当确保当前应用程序域有且仅有一个EtpExceptionBuilderContainer实例。
        /// </summary>
        public static EtpExceptionBuilderContainer Current
        {
            get
            {
                if (etpExceptionBuilderContainer == null)
                {
                    lock (lockObj)
                    {
                        if (etpExceptionBuilderContainer == null)
                        {
                            etpExceptionBuilderContainer = new EtpExceptionBuilderContainer();
                            //初始化缓存集合
                            etpExceptionBuilderContainer.dicCatheBuilder = new Dictionary<string, IEtpExceptionBuilder>();
                            //反射添加缓存
                            OAuthLoginConfig authLoginConfig = (OAuthLoginConfig)ConfigurationManager.GetSection("OAuthLogin");
                            var builders = authLoginConfig.EtpExceptionBuilder;
                            //遍历不同平台的程序集
                            for (int i = 0; i < builders.Count; i++)
                            {
                                Assembly assembly = Assembly.Load(builders[i].Assembly);
                                //反射创建提供程序实例
                                object obj = assembly.CreateInstance(builders[i].Type);
                                etpExceptionBuilderContainer.dicCatheBuilder.Add(builders[i].Etp, (IEtpExceptionBuilder)obj);
                            }
                        }
                    }
                }
                return etpExceptionBuilderContainer;
            }
        }

    }//end EtpExceptionBuilderContainer

}//end namespace OAuthLogin