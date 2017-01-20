using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace OAuthLogin
{
    /// <summary>
    /// 表示授权成功后平台返回的数据。
    /// </summary>
    public class AuthorizationResult
    {

        /// <summary>
        /// 访问令牌
        /// </summary>
        private string _token;
        /// <summary>
        /// token的类型
        /// </summary>
        private string _tokenType = "Bearer";
        /// <summary>
        /// token到期时间
        /// </summary>
        private DateTime _expireAt;
        /// <summary>
        /// 刷新令牌
        /// </summary>
        private string _refreshToken;
        /// <summary>
        /// 刷新令牌的到期时间
        /// </summary>
        private DateTime _refreshExpireAt;
        /// <summary>
        /// 用户在Etp的用户名
        /// </summary>
        private string _userName;
        /// <summary>
        /// 用户在Etp的唯一标识
        /// </summary>
        private string _openId;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("访问令牌是:{0}",_token);
            sb.AppendFormat("token的类型是:{0}", _tokenType);
            sb.AppendFormat("token到期时间是:{0}", _expireAt);
            sb.AppendFormat("刷新令牌是:{0}", _refreshToken);
            sb.AppendFormat("刷新令牌的到期时间是:{0}", _refreshExpireAt);
            sb.AppendFormat("用户在Etp的用户名:{0}", _userName);
            sb.AppendFormat("唯一标识是:{0}", _openId);
            return sb.ToString();
        }

        /// <summary>
        /// 获取Token值。
        /// </summary>
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
            }
        }

        /// <summary>
        /// 获取token的类型
        /// </summary>
        public string TokenType
        {
            get
            {
                return _tokenType;
            }
            set
            {
                _tokenType = value;
            }
        }

        /// <summary>
        /// 获取Token失效时间。
        /// </summary>
        public DateTime ExpireAt
        {
            get
            {
                return _expireAt;
            }
            set
            {
                _expireAt = value;
            }
        }

        /// <summary>
        /// 获取刷新令牌。
        /// </summary>
        public string RefreshToken
        {
            get
            {
                return _refreshToken;
            }
            set
            {
                _refreshToken = value;
            }
        }

        /// <summary>
        /// 获取刷新令牌的到期时间。
        /// </summary>
        public DateTime RefreshExpireAt
        {
            get
            {
                return _refreshExpireAt;
            }
            set
            {
                _refreshExpireAt = value;
            }
        }

        /// <summary>
        /// 获取用户在Etp的用户名。
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        /// <summary>
        /// 获取用户在平台的唯一标识。
        /// </summary>
        public string OpenId
        {
            get
            {
                return _openId;
            }
            set
            {
                _openId = value;
            }
        }

    }//end AuthorizationResult

}//end namespace OAuthLogin