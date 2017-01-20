using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OAuthLogin
{
	/// <summary>
	/// 适用于美丽说的EtpException构造器。
	/// </summary>
	public class MeiLiShuoExceptionBuilder : IEtpExceptionBuilder {

		public MeiLiShuoExceptionBuilder(){

		}

		~MeiLiShuoExceptionBuilder(){

		}

		/// <summary>
		/// 创建一个EtpException实例，该实例封装Etp返回的错误消息。
		/// </summary>
		/// <param name="code">主错误码。</param>
		/// <param name="description">主错误描述。</param>
		/// <param name="subCode">子错误码。</param>
		/// <param name="subDescription">子错误描述。</param>
		public EtpException Create(string code, string description, string subCode = "", string subDescription = ""){
            EtpException etpException = null;
            int error_code;
            if (code == "0000001")

            {
                //平台级异常
                etpException = EtpException.CreatePlatformException(true);
            }
            else if (code.StartsWith("00"))

            {
                //平台级异常
                etpException = EtpException.CreatePlatformException(false);
            }
            else if (int.TryParse(code, out error_code))

            {
                etpException = EtpException.CreateBusinessException();
            }
            else

            {
                //应用级异常
                etpException = EtpException.CreateApplicationException();
            }
            //设置主错误信息
            etpException.SetError(code, description);
            //设置子错误信息
            etpException.SetSubError(subCode, subDescription);

            return etpException;
        }

	}//end MeiLiShuoExceptionBuilder

}//end namespace OAuthLogin