/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�SQLManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��56��
 * * �ļ���ʶ��A6F929D1-E20E-4C5D-A780-396ACADF020D
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// SQL Server���ݿ������.
    /// </summary>
    public class SQLManager : DataManager
    {
        private string database;
        private string username;
        private string password;
        private SqlConnection conn; // ���ݿ�����
        private int lastUpdaterId = 0;

        /// <summary>
        /// ���캯��
        /// </summary>
        public SQLManager()
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dataSource">SQL Server����Դ��IP��ַ��.</param>
        /// <param name="database">������.</param>
        /// <param name="usr">�û���.</param>
        /// <param name="pwd">����.</param>
        public SQLManager(string dataSource, string database, string usr, string pwd)
        {
            this.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
        }

        /// <summary>
        /// ������.
        /// </summary>
        public string Database
        {
            get { return this.database; }
            set { this.database = value; }
        }

        /// <summary>
        /// ����SQL Server���ݿ���û���.
        /// </summary>
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        /// <summary>
        /// ����SQL Server���ݿ������.
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
        /// �ͷ��ɵ�ǰ������Ƶ�������Դ.
        /// </summary>
        /// <param name="disposing">��ʽ����.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // �ͷ��й���Դ
            }

            // �ͷŷ��й���Դ
            this.Close();
            base.Dispose(disposing);
        }

        /// <summary>
        /// �����ݿ�����
        /// </summary>
        /// <param name="dataSource">SQL Server����Դ��IP��ַ��.</param>
        /// <param name="database">������.</param>
        /// <param name="usr">�û���.</param>
        /// <param name="pwd">����.</param>
        public void Open(string dataSource, string database, string usr, string pwd)
        {
            this.DataSource = dataSource;
            this.database = database;
            this.username = usr;
            this.password = pwd;
            this.Open();
        }

        /// <summary>
        /// �����ݿ�����.
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
        /// ʹ�����ݿ������ַ��������ݿ�.
        /// </summary>
        public override void OpenByConnString()
        {
            this.conn = new SqlConnection();
            this.conn.ConnectionString = this.ConnString;
            this.conn.Open();
        }

        /// <summary>
        /// �ر�SQL����.
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
        /// ִ��SQL���.
        /// </summary>
        /// <param name="sQLCmd">SQL���.</param>
        /// <returns>��Ӱ�������.</returns>
        public override int Exec(string sQLCmd)
        {
            SqlCommand cmd = new SqlCommand(sQLCmd, conn);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ִ��SQL��䣬����Ӧ��������䵽DataTable�У����ܽ��и��²���.
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
        /// ѡ��һ����Χ��¼��Select���.
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
        /// ʵ�ַ�ҳ��Select.
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
        /// ִ��ɾ��������SQL���.
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
        /// ����һ���µ�SQL������.
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
