using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OAuthLogin
{
    /// <summary>
    /// EtpException建造者接口。
    /// </summary>
    public interface IEtpExceptionBuilder
    {

        /// <summary>
        /// 创建一个EtpException实例，该实例封装Etp返回的错误消息。
        /// </summary>
        /// <param name="code">主错误码。</param>
        /// <param name="description">主错误描述。</param>
        /// <param name="subCode">子错误码。</param>
        /// <param name="subDescription">子错误描述。</param>
        EtpException Create(string code, string description, string subCode = "", string subDescription = "");
    }//end IEtpExceptionBuilder

}//end namespace OAuthLogin