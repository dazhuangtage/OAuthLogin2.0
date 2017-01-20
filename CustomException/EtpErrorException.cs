using System;

namespace OAuthLogin
{
    /// <summary>
    /// Description of EtpErrorException.
    /// </summary>
    [Obsolete]
    public abstract class EtpErrorException : Exception
    {
        #region 成员属性

        /// <summary>
        /// 错误主代码
        /// </summary>
        protected string _code;

        /// <summary>
        /// 错误主描述
        /// </summary>
        protected string _description;

        /// <summary>
        /// 错误子代码
        /// </summary>
        protected string _subCode;

        /// <summary>
        /// 错误子代码格式
        /// </summary>
        protected string _subCodePattern;

        /// <summary>
        ///  错误子描述
        /// </summary>
        protected string _subDescription;

        #endregion

        private string _messageFormat = "错误代码: {0}, 错误信息: {1}; 子代码: {2}, 子信息: {3}";

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

        /// <summary>
        /// 错误子代码
        /// </summary>
        public string SubCode
        {
            get { return _subCode; }
            internal set { _subCode = value; }
        }

        /// <summary>
        /// 错误子代码格式
        /// </summary>
        public string SubCodePattern
        {
            get { return _subCodePattern; }
            internal set { _subCodePattern = value; }
        }

        /// <summary>
        ///  错误子描述
        /// </summary>
        public string SubDescription
        {
            get { return _subDescription; }
            internal set { _subDescription = value; }
        }

        public override string Message
        {
            get
            {
                return string.Format(_messageFormat, new object[] {
                    _code,
                    _description,
                    _subCode,
                    _subDescription
                });
            }
        }

        #endregion

        protected EtpErrorException() : base() { }

        //protected EtpErrorException(string message, bool retriable) : base(message, retriable){}
    }
}
