using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Custom.Data.SqlClient;
//using Custom.Data;
using System.Data;

namespace OAuthLogin
{
    [Obsolete]
    public class EtpApplication // : SqlPersistent, IPersistent
    {
        #region 属性访问器
        /// <summary>
        /// 应用的唯一标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 应用创建的时间
        /// </summary>
        public DateTime CreatedTime { get; private set; }

        /// <summary>
        /// 应用所在的Etp的枚举数字
        /// </summary>
        public Byte EtpEnumerator { get; set; }

        /// <summary>
        /// 应用最后一次修改的时间
        /// </summary>
        public DateTime ModifiedTime { get; private set; }

        /// <summary>
        /// 应用的签名密钥
        /// </summary>
        public String Secret { get; set; }
        #endregion

        #region 构造方法
        /// <summary>
        /// 创建EtpApplication类的实例，该实例已实施过持久化操作
        /// </summary>
        public EtpApplication()
            : base()
        {
            CreatedTime = DateTime.Now;
        }

        ///// <summary>
        ///// 创建EtpApplication类的实例，该实例已实施过持久化操作
        ///// </summary>
        ///// <param name="appKey">应用在Etp的唯一标识</param>
        ///// <param name="etpEnumerator">应用所在的Etp的枚举数字</param>
        //public EtpApplication(String appKey, Byte etpEnumerator)
        //    : base(new object[] { appKey, etpEnumerator })
        //{
        //    AppKey = appKey;
        //    EtpEnumerator = etpEnumerator;
        //}
        #endregion

        #region 重写基类方法
        //protected override void AppendToDB(IDataManager dataManager)
        //{
        //    string sql_ins = "INSERT INTO [EtpApplication]([AppKey],[CreatedTime],[EtpEnumerator],[ModifiedTime],[Secret])"+
        //        " VALUES('{0}','{1}',{2},'{3}','{4}')";
        //    sql_ins = string.Format(sql_ins, AppKey,CreatedTime,EtpEnumerator,
        //        ModifiedTime.Equals(new DateTime())? "NULL" : ModifiedTime.ToString(),Secret);
        //    sql_ins = sql_ins.Replace("'NULL'", "NULL");
        //    dataManager.Execute(sql_ins);
        //}

        //protected override bool ReadFromDB(IDataManager dataManager)
        //{
        //    bool flag = false;
        //    string sql_slt = "SELECT * FROM [EtpApplication] WHERE [AppKey]='{0}' AND [EtpEnumerator]={1}";
        //    sql_slt = string.Format(sql_slt, AppKey, EtpEnumerator);
        //    dataManager.OpenConnection();
        //    IDataReader dr = dataManager.ExecuteReader(sql_slt);
        //    if (dr.Read())
        //    {
        //        CreatedTime = (DateTime)dr["CreatedTime"];
        //        if (dr["ModifiedTime"] != DBNull.Value) ModifiedTime = (DateTime)dr["ModifiedTime"];
        //        Secret = (string)dr["Secret"];

        //        flag = true;
        //    }
        //    dr.Close();
        //    dataManager.CloseConnection();
        //    return flag;
        //}

        //protected override void UpdateDB(IDataManager dataManager)
        //{
        //    string sql_upd = "UPDATE [EtpApplication] SET [ModifiedTime]='{0}',[Secret]='{1}' WHERE [AppKey]='{2}' "+
        //        "AND [EtpEnumerator]={3} ";
        //    sql_upd = string.Format(sql_upd, DateTime.Now, Secret, AppKey, EtpEnumerator);
        //    dataManager.Execute(sql_upd);
        //}

        //protected override void DeleteFromDB(IDataManager dataManager)
        //{
        //    string sql_del = "DELETE FROM [EtpApplication]  WHERE [AppKey]='{0}' AND [EtpEnumerator]={1}";
        //    sql_del = string.Format(sql_del, AppKey, EtpEnumerator);
        //    dataManager.Execute(sql_del);
        //}
        #endregion
    }
}
