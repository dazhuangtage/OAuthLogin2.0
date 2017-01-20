using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OAuthLogin
{
    /// <summary>
    /// 适用于阿里巴巴的EtpException构造器。
    /// </summary>
    public class AlibabaExceptionBuilder : IEtpExceptionBuilder
    {

        public AlibabaExceptionBuilder()
        {

        }

        ~AlibabaExceptionBuilder()
        {

        }

        /// <summary>
        /// 创建一个EtpException实例，该实例封装Etp返回的错误消息。
        /// </summary>
        /// <param name="code">主错误码。</param>
        /// <param name="description">主错误描述。</param>
        /// <param name="subCode">子错误码。</param>
        /// <param name="subDescription">子错误描述。</param>
        public EtpException Create(string code, string description, string subCode = "", string subDescription = "")
        {
            EtpException etpException = null;

            //通过code判断不同级别EtpException实例
            if (code == "invalid_request")
            {
                etpException = EtpException.CreateApplicationException();
            }
            else if (subCode.Contains("090008"))
            {
                etpException = EtpException.CreatePlatformException(true);
            }
            else if (code == "403" || code == "400" || code == "unauthorized_client" || code == "401")
            {
                etpException = EtpException.CreatePlatformException(false);
            }
            else
            {
                etpException = EtpException.CreateBusinessException();
            }

            etpException.SetError(code, description);
            etpException.SetSubError(subCode, subDescription);
            return etpException;
        }

    }//end AlibabaExceptionBuilder

}//end namespace OAuthLogin