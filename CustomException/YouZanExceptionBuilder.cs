using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace OAuthLogin
{
    /// <summary>
    /// 适用于有赞的EtpException构造器。
    /// </summary>
    public class YouZanExceptionBuilder : IEtpExceptionBuilder
    {


        /// <summary>
        /// 平台级错误Code
        /// </summary>
        private readonly string[] _platformErrorCode = { "40003", "-1" };

        /// <summary>
        /// 应用级错误Code
        /// </summary>
        private readonly string[] _appErrorCode = { "40001", "40002", "40004", "40005", "40006", "40007", "40008", "40009", "40010", "40011", "40012", "41000", "50000" };

        /// <summary>
        /// 业务级错误Code
        /// </summary>
        private readonly string[] _businessErrorCode = { };

        public YouZanExceptionBuilder()
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
            //平台级错误Code
            if (_platformErrorCode.Contains(code))
            {
                etpException = EtpException.CreatePlatformException(true);
                etpException.SetError(code, description);
                etpException.SetSubError(subCode, subDescription);
            }
            //应用级错误Code
            else if (_appErrorCode.Contains(code))
            {
                etpException = EtpException.CreateApplicationException();
                etpException.SetError(code, description);
                etpException.SetSubError(subCode, subDescription);
            }
            return etpException;
        }

    }//end YouZanExceptionBuilder

}//end namespace OAuthLogin