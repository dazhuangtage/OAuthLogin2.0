using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace OAuthLogin
{
    /// <summary>
    /// 适用于京东的EtpException构造器。
    /// </summary>
    public class JdExceptionBuilder : IEtpExceptionBuilder
    {

        /// <summary>
        /// 平台级错误Code
        /// </summary>
        private readonly string[] _platformErrorCode = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "43" };

        /// <summary>
        /// 平台级子错误Code
        /// </summary>
        private readonly string[] _platformChildErrorCode = { "50", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73" };

        /// <summary>
        /// 应用级错误Code
        /// </summary>
        private readonly string[] _appErrorCode = { "101" };

        /// <summary>
        /// 业务级错误Code
        /// </summary>
        private readonly string[] _businessErrorCode = { "201", "202", "203", "204", "205", "206", "207", "208", "209", "301", "302", "303", "304", "305", "251", "401", "402", "403", "404", "405" };

        public JdExceptionBuilder()
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
            //平台级
            if (_platformErrorCode.Contains(code))
            {
                etpException = EtpException.CreatePlatformException(true);
                etpException.SetError(code, description);
                etpException.SetSubError(subCode, subDescription);
            }
            //应用级
            else if (_appErrorCode.Contains(code))
            {
                etpException = EtpException.CreateApplicationException();
                etpException.SetError(code, description);
                etpException.SetSubError(subCode, subDescription);
            }
            //业务级
            else if (_businessErrorCode.Contains(code))
            {
                etpException = EtpException.CreateBusinessException();
                etpException.SetError(code, description);
                etpException.SetSubError(subCode, subDescription);
            }
            return etpException;
        }

    }//end JdExceptionBuilder

}//end namespace OAuthLogin