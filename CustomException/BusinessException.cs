using System;

namespace OAuthLogin
{
    /// <summary>
    /// 由业务级错误引发的异常，简称业务级异常
    /// 业务级错误包含两类：
    /// 1.由应用级参数缺失、有误或格式错误等原因造成的错误，错误码为40,41的错误；40主要是必填参数没有传入报错，41主要是传入的参数格式不对报错
    /// 2.错误码大于100或者等于15，且子错误码（sub_code）是"isv."开头的错误
    /// </summary>
    [Obsolete]
    public class BusinessException : EtpErrorException
    {
        internal BusinessException()
            : base()
        {
        }

        //internal BusinessException(string message, bool retriable) : base(message , retriable)
        //{
        //    _retriable = retriable;
        //}
    }
}
