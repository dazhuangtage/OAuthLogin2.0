using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OAuthLogin
{
    /// <summary>
    /// 适用于淘宝的EtpException构造器。
    /// 参考资料：http://open.taobao.com/doc2/detail?spm=a219a.7386781.3.5.
    /// mYMgk6&articleId=101645&docType=1&treeId=1
    /// </summary>
    public class TaobaoExceptionBuilder : IEtpExceptionBuilder
    {

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
            int errCode;
            if (int.TryParse(code, out errCode))
            {
                //根据不同的错误类型创建不同的异常对象
                if (((errCode > 100 || errCode == 15) && subCode.StartsWith("isv.")) || errCode == 40 || errCode == 41)
                {
                    //业务级异常
                    etpException = EtpException.CreateBusinessException();
                }
                else if (((errCode < 100) && (!(errCode == 15 || errCode == 40 || errCode == 41))))
                {
                    //一般是由于用户的请求不符合各种基本校验而引起的。用户遇到这些错误的返回首先检查应用的权限、频率等情况，然后参照文档检验一下传入的参数是否完整且合法.
                    etpException = EtpException.CreatePlatformException(false);
                }
                else if (subCode.StartsWith("isp."))
                {
                    //一般是由于服务端异常引起的。用户遇到这类错误需要隔一段时间再重试就可以解决。
                    etpException = EtpException.CreatePlatformException(true);
                }
                else
                {
                    etpException = EtpException.CreateApplicationException();
                }
            }
            else
            {
                etpException = EtpException.CreateBusinessException();
            }
            etpException.SetError(code, description);
            etpException.SetSubError(subCode, subDescription);
            return etpException;
        }

    }//end TaobaoExceptionBuilder

}//end namespace OAuthLogin