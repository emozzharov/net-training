/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：OleDbManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分50秒
 * * 文件标识：CF457278-457E-44F7-9E95-E873F4FFDF2F
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace Codest.Data
{
    /// <summary>
    /// OleDb(默认Access)数据库数据库管理器.
    /// </summary>
    public class OleDbManager : DataManager
    {
        private System.Data.OleDb.OleDbConnection conn; // access数据库连接
        private int lastUpdaterId;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public OleDbManager()
        {
            this.lastUpdaterId = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">设置数据源.</param>
        public OleDbManager(string dataSource)
            : this()
        {
            this.DataSource = dataSource;
        }

        /// <summary>
        /// Gets oledbconnection.
        /// </summary>
        internal System.Data.OleDb.OleDbConnection Connection => this.conn;

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
                this.DataSource = null;
            }

            // 释放非托管资源
            this.Close();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 打开指定Access数据库文件.
        /// </summary>
        /// <param name="dataSource">data source param.</param>
        public void Open(string dataSource)
        {
            this.DataSource = dataSource;
            this.Open();
        }

        /// <summary>
        /// 打开已经设定好的Access数据库文件.
        /// </summary>
        public override void Open()
        {
            this.ConnString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + this.DataSource;
            this.OpenByConnString();
        }

        /// <summary>
        /// 使用数据库连接字符串打开数据库.
        /// </summary>
        public override void OpenByConnString()
        {
            this.conn = new OleDbConnection();
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
            OleDbCommand cmd = new OleDbCommand(sQLCmd, this.conn);
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
            OleDbDataAdapter da;
            DataTable dt = new DataTable();
            da = new OleDbDataAdapter(sQLCmd, conn);
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
            OleDbDataAdapter da;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            da = new OleDbDataAdapter(sQLCmd, conn);
            da.Fill(ds, startRecord, maxRecord, srcTalbe);
            dt = ds.Tables[0];
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
        /// <returns>Some return.</returns>
        public override bool Delete(string sQLCmd)
        {
            if (sQLCmd?.Substring(0, 6).ToLower() != "delete")
            {
                return false;
            }

            this.ExecNum++;
            OleDbCommand cmd;
            cmd = new OleDbCommand(sQLCmd, conn);
            cmd.ExecuteNonQuery();

            // DecideRelease();
            return true;
        }

        /// <summary>
        /// 分配一个新的更新器.
        /// </summary>
        /// <returns>OleDb更新器.</returns>
        public override DataUpdater AllocateDataUpdater()
        {
            int id = this.lastUpdaterId++;
            OleDbUpdater updater = new OleDbUpdater(id, this);
            this.DataUpdaterColl.Add(id, updater);
            return updater;
        }
    }
}