using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace OAuthLogin
{
    /// <summary>
    /// 表示Etp返回的错误信息
    /// </summary>
    public sealed class EtpException : System.Exception
    {

        /// <summary>
        /// 表示错误发生后是否可以重试
        /// </summary>
        private bool _retriable = false;
        /// <summary>
        /// 主错误码
        /// </summary>
        private string _code;
        /// <summary>
        /// 主错误描述
        /// </summary>
        private string _description;
        /// <summary>
        /// 子错误码
        /// </summary>
        private string _subCode;
        /// <summary>
        /// 子错误码
        /// </summary>
        private string _subDescription;
        /// <summary>
        /// 错误类型
        /// </summary>
        private eErrorType _errorType;

        /// <summary>
        /// 创建EtpException实例。
        /// </summary>
        private EtpException()
        {

        }

        /// <summary>
        /// 获取主错误码。
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
        }

        /// <summary>
        /// 获取主错误描述。
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
        }

        /// <summary>
        /// 获取子错误码。
        /// </summary>
        public string SubCode
        {
            get
            {
                return _subCode;
            }
        }

        /// <summary>
        /// 获取子错误码描述。
        /// </summary>
        public string SubDescription
        {
            get
            {
                return _subDescription;
            }
        }

        /// <summary>
        ///获取异常信息。
        ///异常消息格式为：子错误描述（错误码：主错误码，子错误码）。
		/// </summary>
		public override string Message
        {
            get { return string.Format("{0}(错误码:{1},{2})", SubDescription, Code, SubCode); }
        }

        /// <summary>
        /// 获取错误类型。
        /// </summary>
        public eErrorType ErrorType
        {
            get
            {
                return _errorType;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示错误发生后是否可重试。
        /// </summary>
        public bool Retriable
        {
            get
            {
                return _retriable;
            }
        }

        /// <summary>
        /// 创建一个EtpException实例，该实例表示调用Api时发生了业务级异常。
        /// </summary>
        internal static EtpException CreateBusinessException()
        {
            EtpException etpException = new EtpException();
            //设置业务级异常
            etpException._errorType = eErrorType.BusinessError;
            return etpException;
        }

        /// <summary>
        /// 创建一个EtpException实例，该实例表示调用Api时发生了应用级异常。
        /// </summary>
        internal static EtpException CreateApplicationException()
        {
            EtpException etpException = new EtpException();
            //设置应用级异常
            etpException._errorType = eErrorType.ApplicationError;
            return etpException;
        }

        /// <summary>
        /// 创建一个EtpException实例，该实例表示调用Api时发生了平台级异常。
        /// </summary>
        /// <param name="retriable">指示调用方是否可以重新尝试调用。</param>
        internal static EtpException CreatePlatformException(bool retriable)
        {
            EtpException etpException = new EtpException();
            //设置是否重试
            etpException._retriable = retriable;
            //设置平台及异常
            etpException._errorType = eErrorType.PlatformError;
            return etpException;
        }

        /// <summary>
        /// 设置主错误信息。
        /// </summary>
        /// <param name="code">主错误码。</param>
        /// <param name="description">主错误描述。</param>
        internal void SetError(string code, string description)
        {
            //设置主错误码
            this._code = code;
            //设置主错误信息
            this._description = description;
        }

        /// <summary>
        /// 设置子错误信息。
        /// </summary>
        /// <param name="subCode">子错误码。</param>
        /// <param name="subDescription">子错误描述。</param>
        internal void SetSubError(string subCode, string subDescription)
        {
            //设置子错误码
            this._subCode = subCode;
            //设置子错误信息
            this._subDescription = subDescription;
        }



        /// <summary>
        /// 创建一个EtpException实例，用以封装Etp返回的错误消息。
        /// </summary>
        /// <param name="etp">返回错误消息的平台的名称。</param>
        /// <param name="code">主错误码。</param>
        /// <param name="description">主错误描述。</param>
        /// <param name="subCode">子错误码。</param>
        /// <param name="subDescription">子错误描述。</param>
        public static EtpException Create(EtpName etp, string code, string description, string subCode, string subDescription)
        {
            //通过容器返回指定平台的异常构造器
            IEtpExceptionBuilder builder = EtpExceptionBuilderContainer.Current.GetBuilder(etp);
            //创建异常
            return builder.Create(code, description, subCode, subDescription);
        }

    }//end EtpException

}//end namespace OAuthLogin