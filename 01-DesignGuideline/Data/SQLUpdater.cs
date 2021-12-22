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
    /// SQL Server�������ݿ������.
    /// </summary>
    public class SQLUpdater : DataUpdater
    {
        /// <summary>
        /// ��ǰ��������ʹ�õ����ݿ������.
        /// </summary>
        private SQLManager dataManager;

        /// <summary>
        /// ���и��²���ʱ����������������Ϣ.
        /// </summary>
        private SqlDataAdapter dap;

        /// <summary>
        /// ���и��²���ʱ�����DataAdapterʹ��.
        /// </summary>
        private SqlCommandBuilder cmdb;

        /// <summary>
        /// ���캯��.
        /// </summary>
        /// <param name="id">������Ψһ��ID.</param>
        /// <param name="manager">���ݿ������.</param>
        public SQLUpdater(int id, SQLManager manager)
            : base(id)
        {
            this.dataManager = manager;
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
                this.dap.Dispose();
                this.cmdb.Dispose();
                // �ͷ��й���Դ
            }

            // �ͷŷ��й���Դ
            this.dataManager = null;
            this.cmdb = null;
            this.dap = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// ʹ���󽫽����޸�ģʽ״̬
        /// �û������޸ķ��ؽ��������ɾ�����޸ĺ�������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬.
        /// </summary>
        /// <param name="sQLCmd">SQL���.</param>
        /// <returns>��ѯ��Ӧ���.</returns>
        public override DataTable SelectWithUpdate(string sQLCmd)
        {
            this.dataManager.ExecNum++;
            System.Data.DataTable dt = new DataTable();
            this.dap = new SqlDataAdapter(sQLCmd, this.dataManager.Conn);
            this.cmdb = new SqlCommandBuilder(this.dap);
            this.dap.Fill(dt);
            return dt;
        }

        /// <summary>
        /// ʹ��������޸�ģʽ
        /// �û������ڷ��ر�ṹ��DataTable���������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬.
        /// </summary>
        /// <param name="tableName">��Ҫ����ı�����.</param>
        /// <returns>Ҫ����Ŀ���Ľṹ.</returns>
        public override DataTable InsertMode(string tableName)
        {
            this.dataManager.ExecNum++;
            System.Data.DataTable dt = new DataTable();
            this.dap = new SqlDataAdapter("select * from [" + tableName + "] where 1=0", this.dataManager.Conn);
            this.cmdb = new SqlCommandBuilder(this.dap);
            this.dap.Fill(dt);
            return dt;
        }

        /// <summary>
        /// �ر��޸�ģʽ,������DataTable���и��²���.
        /// </summary>
        /// <param name="dataTableSource">Ҫ�ύ�����ݱ�.</param>
        public override void Update(System.Data.DataTable dataTableSource)
        {
            this.dataManager.ExecNum++;
            this.dap.Update(dataTableSource);
            this.DecideRelease();
        }

        /// <summary>
        /// �ͷŵ�ǰ��������
        /// ���AutoRelease=true�������Update()����Զ����ø÷�����.
        /// </summary>
        public override void Release()
        {
            this.dataManager.ReleaseDataUpdater(this);
        }
    }
}
