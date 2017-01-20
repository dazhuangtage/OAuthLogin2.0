
using System;

namespace OAuthLogin
{
    /// <summary>
    /// 由容器类错误引发的异常，简称容器类异常
    /// 容器类错误是错误代码大于或等于100，且子代码为空的错误
    /// </summary>
    [Obsolete]
    public class ContainerException : EtpErrorException
    {
        #region 成员属性

        /// <summary>
        /// 错误主代码
        /// </summary>
        private string _code;

        /// <summary>
        /// 错误主描述
        /// </summary>
        private string _description;
        #endregion

        #region 属性访问器

        /// <summary>
        /// 错误主代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            internal set { _code = value; }
        }

        /// <summary>
        /// 错误主描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            internal set { _description = value; }
        }
        #endregion

        /// <summary>
        /// 创建ContainerException类的实例 
        /// </summary>
        internal ContainerException()
            : base()
        {

      
        }

        //internal  ContainerException(string message) : base(message,false)
        //{
        //}
    }
}
