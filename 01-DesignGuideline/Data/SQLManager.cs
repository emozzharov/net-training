/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：SQLManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分56秒
 * * 文件标识：A6F929D1-E20E-4C5D-A780-396ACADF020D
 * * 内容摘要：
 * *******************************************************************************/


using System.Data;
using System.Data.SqlClient;

namespace Codest.Data
{
    /// <summary>
    /// SQL Server数据库管理器
    /// </summary>
    public class SQLManager : DataManager
    {
        #region 成员变量
        private string database;
        private string username;
        private string password;
        internal SqlConnection connection; //数据库连接
        private int lastUpdaterId = 0;
        #endregion

        #region 接口封装
        /// <summary>
        /// 库名称
        /// </summary>
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        /// <summary>
        /// 连接SQL Server数据库的用户名
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// 连接SQL Server数据库的密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLManager()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">SQL Server数据源（IP地址）</param>
        /// <param name="database">库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        public SQLManager(string dataSource, string database, string user, string password)
        {
            base.DataSource = dataSource;
            this.database = database;
            this.username = user;
            this.password = password;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~SQLManager()
        {
            Dispose(false);
        }
        #endregion

        #region protected override void Dispose(bool disposing)
        /// <summary>
        /// 释放由当前对象控制的所有资源
        /// </summary>
        /// <param name="disposing">显式调用</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                //释放托管资源
            }
            //释放非托管资源
            this.Close();
            base.Dispose(disposing);
        }

        #endregion
        
        //--begin--连接数据库操作--

        #region  public void Open(string dataSource, string database, string usr, string pwd)
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="dataSource">SQL Server数据源（IP地址）</param>
        /// <param name="database">库名称</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
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
        /// 打开数据库连接
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
        /// 使用数据库连接字符串打开数据库
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
        /// 关闭SQL连接
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

        //--end----连接数据库操作--

        //--begin--访问数据库操作--

        #region  public override int Exec(string SQLCmd)
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlCommand">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public override int Execute(string sqlCommand)
        {
            SqlCommand cmd = new SqlCommand(sqlCommand, connection);
            return cmd.ExecuteNonQuery();
        }

        #endregion

        #region public override DataTable Select(string SQLCmd)
        /// <summary>
        /// 执行SQL语句，将响应的数据填充到DataTable中，不能进行更新操作
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
        /// 选择一定范围记录的Select语句
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
        /// 实现分页的Select
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
        /// 执行删除操作的SQL语句
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

        //--end----访问数据库操作--

        //--begin--更新器操作--

        #region  public override DataManager.DataUpdater AllocateDataUpdater()
        /// <summary>
        /// 分配一个新的SQL更新器
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

        //--end----更新器操作--


    }
}
