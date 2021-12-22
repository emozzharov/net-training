/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�OleDbUpdater.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��52��
 * * �ļ���ʶ��61502EEA-229B-4503-9A25-0DD25ED0B4B3
 * * ����ժҪ��
 * *******************************************************************************/



using System.Data;
using System.Data.OleDb;

namespace Codest.Data
{
    /// <summary>
    /// OleDb���ݿ������
    /// </summary>
    public class OleDbUpdater : DataUpdater
    {
        #region ��Ա����
        /// <summary>
        /// ��ǰ��������ʹ�õ����ݿ������
        /// </summary>
        protected OleDbManager dataManager;
        /// <summary>
        /// ���и��²���ʱ����������������Ϣ
        /// </summary>
        protected OleDbDataAdapter dataAdapter;
        /// <summary>
        /// ���и��²���ʱ�����OleDbDataAdapterʹ��
        /// </summary>
        protected OleDbCommandBuilder commandBuilder;
        #endregion

        #region �ӿڷ�װ

        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">������Ψһ�ĸ�����ID</param>
        /// <param name="manager">���ݿ������</param>
        public OleDbUpdater(int id, OleDbManager manager)
            : base(id)
        {
            dataManager = manager;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~OleDbUpdater()
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
            System.Data.DataTable dataTable = new DataTable();
            dataAdapter = new OleDbDataAdapter(sqlCommand, dataManager.connection);
            commandBuilder = new OleDbCommandBuilder(dataAdapter);
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
            dataAdapter = new OleDbDataAdapter("select * from [" + tableName + "] where false", dataManager.connection);
            commandBuilder = new OleDbCommandBuilder(dataAdapter);
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
        /// �ͷŵ�ǰ������
        /// </summary>
        public override void Release()
        {
            dataManager.ReleaseDataUpdater(this);
        }
        #endregion
    }


}
