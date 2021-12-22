/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�SQLManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��56��
 * * �ļ���ʶ��A6F929D1-E20E-4C5D-A780-396ACADF020D
 * * ����ժҪ��
 * *******************************************************************************/


using System.Data;
using System.Data.SqlClient;

namespace Codest.Data
{
    /// <summary>
    /// SQL Server���ݿ������
    /// </summary>
    public class SQLManager : DataManager
    {
        #region ��Ա����
        private string database;
        private string username;
        private string password;
        internal SqlConnection connection; //���ݿ�����
        private int lastUpdaterId = 0;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ������
        /// </summary>
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        /// <summary>
        /// ����SQL Server���ݿ���û���
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// ����SQL Server���ݿ������
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public SQLManager()
        {

        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dataSource">SQL Server����Դ��IP��ַ��</param>
        /// <param name="database">������</param>
        /// <param name="user">�û���</param>
        /// <param name="password">����</param>
        public SQLManager(string dataSource, string database, string user, string password)
        {
            base.DataSource = dataSource;
            this.database = database;
            this.username = user;
            this.password = password;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~SQLManager()
        {
            Dispose(false);
        }
        #endregion

        #region protected override void Dispose(bool disposing)
        /// <summary>
        /// �ͷ��ɵ�ǰ������Ƶ�������Դ
        /// </summary>
        /// <param name="disposing">��ʽ����</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //�ͷ��й���Դ
            }
            //�ͷŷ��й���Դ
            this.Close();
            base.Dispose(disposing);
        }

        #endregion
        
        //--begin--�������ݿ����--

        #region  public void Open(string dataSource, string database, string usr, string pwd)
        /// <summary>
        /// �����ݿ�����
        /// </summary>
        /// <param name="dataSource">SQL Server����Դ��IP��ַ��</param>
        /// <param name="database">������</param>
        /// <param name="user">�û���</param>
        /// <param name="password">����</param>
        public void Open(string dataSource, string database, string user, string password)
        {
            base.DataSource = dataSource;
            this.database = database;
            this.username = user;
            this.password = password;
            this.Open();
        }
        #endregion

        #region public override void Open()
        /// <summary>
        /// �����ݿ�����
        /// </summary>
        public override void Open()
        {
            string connectionString = string.Empty;
            connectionString += "server=" + base.DataSource + ";";
            connectionString += "database=" + this.database + ";";
            connectionString += "uid="+this.username+";";
            connectionString += "pwd=" + this.password;
            base.ConnectionString = connectionString;
            this.OpenByConnectionString();
        }
        #endregion

        #region public override void OpenByConnString()
        /// <summary>
        /// ʹ�����ݿ������ַ��������ݿ�
        /// </summary>
        public override void OpenByConnectionString()
        {
            connection = new SqlConnection();
            connection.ConnectionString = base.ConnectionString;
            connection.Open();
        }
        #endregion

        #region public override void Close()
        /// <summary>
        /// �ر�SQL����
        /// </summary>
        public override void Close()
        {
            try
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
            catch
            {

            }
        }
        #endregion

        //--end----�������ݿ����--

        //--begin--�������ݿ����--

        #region  public override int Exec(string SQLCmd)
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="sqlCommand">SQL���</param>
        /// <returns>��Ӱ�������</returns>
        public override int Execute(string sqlCommand)
        {
            SqlCommand cmd = new SqlCommand(sqlCommand, connection);
            return cmd.ExecuteNonQuery();
        }

        #endregion

        #region public override DataTable Select(string SQLCmd)
        /// <summary>
        /// ִ��SQL��䣬����Ӧ��������䵽DataTable�У����ܽ��и��²���
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public override DataTable Select(string sqlCommand)
        {
            executionNumber++;
            SqlDataAdapter dataAdapter;
            DataTable dt = new DataTable();
            dataAdapter = new SqlDataAdapter(sqlCommand, connection);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
            //DecideRelease();
            return dt;
        }
        #endregion

        #region public override DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord)
        /// <summary>
        /// ѡ��һ����Χ��¼��Select���
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="sourceTable"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <returns></returns>
        public override DataTable Select(string sqlCommand, string sourceTable, int startRecord, int maxRecords)
        {
            executionNumber++;
            SqlDataAdapter dataAdapter;
            DataTable dataTable = new DataTable();
            DataSet dataSet = new DataSet();
            dataAdapter = new SqlDataAdapter(sqlCommand, connection);
            dataAdapter.Fill(dataSet, startRecord, maxRecords, sourceTable);
            dataTable = dataSet.Tables[0];
            dataAdapter.Dispose();
            //DecideRelease();
            return dataTable;
        }
        #endregion

        #region public override DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID)
        /// <summary>
        /// ʵ�ַ�ҳ��Select
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="sourceTable"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public override DataTable SelectPage(string sqlCommand, string sourceTable, int pageSize, int pageId)
        {
            if (pageId == 0) pageId = 1;
            return Select(sqlCommand, sourceTable, pageSize * (pageId - 1), pageSize);
        }
        #endregion

        #region public override bool Delete(string SQLCmd)
        /// <summary>
        /// ִ��ɾ��������SQL���
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public override bool Delete(string sqlCommand)
        {
            if (sqlCommand.Substring(0, 6).ToLower() != "delete") return false;
            executionNumber++;
            SqlCommand command;
            command = new SqlCommand(sqlCommand, connection);
            command.ExecuteNonQuery();
            //DecideRelease();
            return true;
        }
        #endregion

        //--end----�������ݿ����--

        //--begin--����������--

        #region  public override DataManager.DataUpdater AllocateDataUpdater()
        /// <summary>
        /// ����һ���µ�SQL������
        /// </summary>
        /// <returns></returns>
        public override DataUpdater AllocateDataUpdater()
        {
            int id = lastUpdaterId++;
            SQLUpdater updater = new SQLUpdater(id, this);
            base.dataUpdaterCollection.Add(id, updater);
            return updater;
        }
        #endregion

        //--end----����������--


    }
}
