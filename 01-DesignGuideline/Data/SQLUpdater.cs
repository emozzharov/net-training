/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�SQLUpdater.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��58��
 * * �ļ���ʶ��9FA64F08-E37B-4579-A18B-DEA743CFED03
 * * ����ժҪ��
 * *******************************************************************************/



using System.Data;
using System.Data.SqlClient;

namespace Codest.Data
{
    /// <summary>
    /// SQL Server�������ݿ������
    /// </summary>
    public class SQLUpdater : DataUpdater
    {
        #region ��Ա����
        /// <summary>
        /// ��ǰ��������ʹ�õ����ݿ������
        /// </summary>
        protected SQLManager dataManager;
        /// <summary>
        /// ���и��²���ʱ����������������Ϣ
        /// </summary>
        protected SqlDataAdapter dataAdapter; 
        /// <summary>
        /// ���и��²���ʱ�����DataAdapterʹ��
        /// </summary>
        protected SqlCommandBuilder commandBuilder;
        #endregion

        #region �ӿڷ�װ

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">������Ψһ��ID</param>
        /// <param name="manager">���ݿ������</param>
        public SQLUpdater(int id, SQLManager manager)
            : base(id)
        {
            dataManager = manager;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~SQLUpdater()
        {
            this.Release();
        }
        #endregion

        #region  protected override void Dispose(bool disposing)
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
            dataManager = null;
            commandBuilder = null;
            dataAdapter = null;
            base.Dispose(disposing);
        }
        #endregion

        #region public override DataTable SelectWithUpdate(string SQLCmd)
        /// <summary>
        /// ʹ���󽫽����޸�ģʽ״̬
        /// �û������޸ķ��ؽ��������ɾ�����޸ĺ�������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬
        /// </summary>
        /// <param name="sqlCommand">SQL���</param>
        /// <returns>��ѯ��Ӧ���</returns>
        public override DataTable SelectWithUpdate(string sqlCommand)
        {
            dataManager.executionNumber++;
            DataTable dataTable = new DataTable();
            dataAdapter = new SqlDataAdapter(sqlCommand, dataManager.connection);
            commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Fill(dataTable);
            return dataTable;
        }
        #endregion

        #region public override DataTable InsertMode(string TableName)
        /// <summary>
        /// ʹ��������޸�ģʽ
        /// �û������ڷ��ر�ṹ��DataTable���������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬
        /// </summary>
        /// <param name="tableName">��Ҫ����ı�����</param>
        /// <returns>Ҫ����Ŀ���Ľṹ</returns>
        public override DataTable InsertMode(string tableName)
        {
            dataManager.executionNumber++;
            DataTable dataTable = new DataTable();
            dataAdapter = new SqlDataAdapter("select * from [" + tableName + "] where 1=0", dataManager.connection);
            commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Fill(dataTable);
            return dataTable;
        }
        #endregion

        #region public override void Update(System.Data.DataTable DataTableSource)

        /// <summary>
        /// �ر��޸�ģʽ,������DataTable���и��²���
        /// </summary>
        /// <param name="dataTableSource">Ҫ�ύ�����ݱ�</param>
        public override void Update(DataTable dataTableSource)
        {
            dataManager.executionNumber++;
            dataAdapter.Update(dataTableSource);
            ReleaseDecide();
        }
        #endregion

        #region  public override void Release()
        /// <summary>
        /// �ͷŵ�ǰ��������
        /// ���AutoRelease=true�������Update()����Զ����ø÷�����
        /// </summary>
        public override void Release()
        {
            dataManager.ReleaseDataUpdater(this);
        }
        #endregion
    }
}
