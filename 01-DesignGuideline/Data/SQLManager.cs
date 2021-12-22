/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：SQLManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分56秒
 * * 文件标识：A6F929D1-E20E-4C5D-A780-396ACADF020D
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// SQL Server数据库管理器.
    /// </summary>
    public class SQLManager : DataManager
    {
        private string database;
        private string username;
        private string password;
        private SqlConnection conn; // 数据库连接
        private int lastUpdaterId = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLManager()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">SQL Server数据源（IP地址）.</param>
        /// <param name="database">库名称.</param>
        /// <param name="usr">用户名.</param>
        /// <param name="pwd">密码.</param>
        public SQLManager(string dataSource, string database, string usr, string pwd)
        {
            this.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
        }

        /// <summary>
        /// 库名称.
        /// </summary>
        public string Database
        {
            get { return this.database; }
            set { this.database = value; }
        }

        /// <summary>
        /// 连接SQL Server数据库的用户名.
        /// </summary>
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        /// <summary>
        /// 连接SQL Server数据库的密码.
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        /// <summary>
        /// Gets conn.
        /// </summary>
        internal SqlConnection Conn
        {
            get { return this.conn; }
        }

        /// <summary>
        /// 释放由当前对象控制的所有资源.
        /// </summary>
        /// <param name="disposing">显式调用.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // 释放托管资源
            }

            // 释放非托管资源
            this.Close();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="dataSource">SQL Server数据源（IP地址）.</param>
        /// <param name="database">库名称.</param>
        /// <param name="usr">用户名.</param>
        /// <param name="pwd">密码.</param>
        public void Open(string dataSource, string database, string usr, string pwd)
        {
            this.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
            this.Open();
        }

        /// <summary>
        /// 打开数据库连接.
        /// </summary>
        public override void Open()
        {
            string connstr = string.Empty;
            connstr += "server=" + this.DataSource + ";";
            connstr += "database=" + this.database + ";";
            connstr += "uid=" + this.username + ";";
            connstr += "pwd=" + this.password;
            this.ConnString = connstr;
            this.OpenByConnString();
        }

        /// <summary>
        /// 使用数据库连接字符串打开数据库.
        /// </summary>
        public override void OpenByConnString()
        {
            this.conn = new SqlConnection();
            this.conn.ConnectionString = this.ConnString;
            this.conn.Open();
        }

        /// <summary>
        /// 关闭SQL连接.
        /// </summary>
        public override void Close()
        {
            try
            {
                this.conn.Close();
                this.conn.Dispose();
                this.conn = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行SQL语句.
        /// </summary>
        /// <param name="sQLCmd">SQL语句.</param>
        /// <returns>受影响的行数.</returns>
        public override int Exec(string sQLCmd)
        {
            SqlCommand cmd = new SqlCommand(sQLCmd, conn);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行SQL语句，将响应的数据填充到DataTable中，不能进行更新操作.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>some return.</returns>
        public override DataTable SelectDataTable(string sQLCmd)
        {
            this.ExecNum++;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(sQLCmd, conn);
            da.Fill(dt);
            da.Dispose();

            // DecideRelease();
            return dt;
        }

        /// <summary>
        /// 选择一定范围记录的Select语句.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <param name="srcTalbe">srcTable param.</param>
        /// <param name="startRecord">startRecord param.</param>
        /// <param name="maxRecord">max record param.</param>
        /// <returns>some return.</returns>
        public override DataTable SelectDataTable(string sQLCmd, string srcTalbe, int startRecord, int maxRecord)
        {
            this.ExecNum++;
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(sQLCmd, conn);
            da.Fill(ds, startRecord, maxRecord, srcTalbe);
            DataTable dt = ds.Tables[0];
            da.Dispose();

            // DecideRelease();
            return dt;
        }

        /// <summary>
        /// 实现分页的Select.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <param name="srcTalbe">srcTable param.</param>
        /// <param name="pageSize">startRecord param.</param>
        /// <param name="pageID">max record param.</param>
        /// <returns>some return.</returns>
        public override DataTable SelectPage(string sQLCmd, string srcTalbe, int pageSize, int pageID)
        {
            if (pageID == 0)
            {
                pageID = 1;
            }

            return this.SelectDataTable(sQLCmd, srcTalbe, pageSize * (pageID - 1), pageSize);
        }

        /// <summary>
        /// 执行删除操作的SQL语句.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>some return.</returns>
        public override bool Delete(string sQLCmd)
        {
            if (sQLCmd?.Substring(0, 6).ToLower() != "delete")
            {
                return false;
            }

            this.ExecNum++;
            SqlCommand cmd;
            cmd = new SqlCommand(sQLCmd, conn);
            cmd.ExecuteNonQuery();

            // DecideRelease();
            return true;
        }

        /// <summary>
        /// 分配一个新的SQL更新器.
        /// </summary>
        /// <returns>some return.</returns>
        public override DataUpdater AllocateDataUpdater()
        {
            int id = this.lastUpdaterId++;
            SQLUpdater updater = new SQLUpdater(id, this);
            this.DataUpdaterColl.Add(id, updater);
            return updater;
        }
    }
}
