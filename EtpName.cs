using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace OAuthLogin
{
    /// <summary>
    /// 定义一个数据结构，用于规范外部交易平台名称。
    /// 名称是业务系统为Etp分配的唯一标识，由不限长度的字符组成。由于极易发生拼写错误，强烈建议在程序中采用如下规范写法：
    /// （1）需要使用某一平台的名称字符串时，应使用其对应的静态字段获取实例，然后使用Name字段获取名称字符串；
    /// （2）需要对一个名称字符串进行处理时，应先使用FromString方法获取其对应的实例，然后使用相应的实例方法进行处理；
    /// （3）扩展机制：如果开发人员要使用未预定义的平台，可以定义自己的枚举器，使用静态字段存储Etp实例。
    /// </summary>
    public struct EtpName
    {

        /// <summary>
        /// Etp的名称
        /// </summary>
        public string Name;

        /// <summary>
        /// Etp的枚举数字
        /// </summary>
        public byte Enumerator;

        /// <summary>
        /// Etp的全名
        /// </summary>
        public string FullName;

        /// <summary>
        /// 淘宝开放平台，名称：top
        /// </summary>
        public static readonly EtpName Top = new EtpName() { Name = "top", Enumerator = 1, FullName = "淘宝" };

        /// <summary>
        /// 阿里巴巴（1688），名称：alibaba
        /// </summary>
        public static readonly EtpName Alibaba = new EtpName() { Name = "alibaba", Enumerator = 2, FullName = "阿里巴巴" };

        /// <summary>
        /// 京东商城，名称：jd
        /// </summary>
        public static readonly EtpName Jd = new EtpName() { Name = "jd", Enumerator = 3, FullName = "京东商城" };

        /// <summary>
        /// 美丽说，名称：meilishuo
        /// </summary>
        public static readonly EtpName MeiLiShuo = new EtpName() { Name = "meilishuo", Enumerator = 4, FullName = "美丽说" };

        /// <summary>
        /// 有赞，名称：youzan
        /// </summary>
        public static readonly EtpName YouZan = new EtpName() { Name = "youzan", Enumerator = 5, FullName = "有赞" };

        /// <summary>
        /// QQ互联,名称:qq
        /// </summary>
        public static readonly EtpName QQ = new EtpName { Name = "qq", Enumerator = 6, FullName = "QQ互联" };

        /// <summary>
        /// 从指定的名称字符串返回相应的EtpName实例。
        /// </summary>
        /// <param name="nameString">名称字条串。</param>
        public static EtpName FromString(string nameString)
        {
            if (nameString == null)
            {
                throw new Exception("平台名称不能为空");
            }
            switch (nameString.ToLower().Trim())
            {
                case "youzan":
                    return YouZan;
                case "meilishuo":
                    return MeiLiShuo;
                case "jd":
                    return Jd;
                case "alibaba":
                    return Alibaba;
                case "top":
                    return Top;
                case "qq":
                    return QQ;
            }
            throw new Exception("平台不存在");
        }

        /// <summary>
        /// 比较当前实例与指定的Etp实例是否等效，是则返回true，否则返回false。
        /// 注：名称字符串相等即为等效，忽略大小写。
        /// </summary>
        /// <param name="etpName">EtpName实例。</param>
        public bool Equals(EtpName etpName)
        {
            if (etpName.FullName.ToLower().Trim() == FullName.ToLower().Trim() && etpName.Name.ToLower().Trim() == Name.ToLower().Trim())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 比较当前实例与指定的名称字符串是否等效，是则返回true，否则返回false。
        /// 注：名称字符串相等即为等效，忽略大小写。
        /// </summary>
        /// <param name="nameString">名称字符串。</param>
        public bool Equals(string nameString)
        {
            if (nameString.ToLower().Trim() == Name.ToLower().Trim())
            {
                return true;
            }
            return false;
        }

    }//end EtpName

}//end namespace OAuthLogin