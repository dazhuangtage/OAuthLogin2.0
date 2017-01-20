using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace OAuthLogin
{
    /// <summary>
    /// 枚举错误类型。
    /// </summary>
    public enum eErrorType : int
    {

        /// <summary>
        /// 业务级错误。
        /// 业务级错误是由于用户输入错误导致的错误，不可重试，要求用户重新输入。
        /// </summary>
        BusinessError,
        /// <summary>
        /// 应用级错误。
        /// 应用级错误是由于业务系统对Api的调用不符合规范导致的错误，不可重试，要求业务系统开发人员修改程序。
        /// </summary>
        ApplicationError,
        /// <summary>
        /// 平台级错误。
        /// 平台级错误是由于平台自身原因导致的错误，是否可重试由平台方提供的错误说明文档确定。
        /// </summary>
        PlatformError

    }//end eErrorType

}//end namespace OAuthLogin