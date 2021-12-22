/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�OleDbUpdater.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��52��
 * * �ļ���ʶ��61502EEA-229B-4503-9A25-0DD25ED0B4B3
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// OleDb���ݿ������.
    /// </summary>
    public class OleDbUpdater : DataUpdater
    {
        /// <summary>
        /// ��ǰ��������ʹ�õ����ݿ������.
        /// </summary>
        private OleDbManager dataManager;

        /// <summary>
        /// ���и��²���ʱ����������������Ϣ.
        /// </summary>
        private System.Data.OleDb.OleDbDataAdapter dap;

        /// <summary>
        /// ���и��²���ʱ�����OleDbDataAdapterʹ��.
        /// </summary>
        private System.Data.OleDb.OleDbCommandBuilder cmdb;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">������Ψһ�ĸ�����ID.</param>
        /// <param name="manager">���ݿ������.</param>
        public OleDbUpdater(int id, OleDbManager manager)
            : base(id)
        {
            this.dataManager = manager;
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
            this.dap = new OleDbDataAdapter(sQLCmd, this.dataManager.Connection);
            this.cmdb = new OleDbCommandBuilder(this.dap);
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
            this.dap = new OleDbDataAdapter("select * from [" + tableName + "] where false", this.dataManager.Connection);
            this.cmdb = new OleDbCommandBuilder(this.dap);
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
        /// �ͷŵ�ǰ������.
        /// </summary>
        public override void Release()
        {
            this.dataManager.ReleaseDataUpdater(this);
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
                // �ͷ��й���Դ
            }

            // �ͷŷ��й���Դ
            this.dataManager = null;
            this.cmdb = null;
            this.dap = null;
            this.dap.Dispose();
            base.Dispose(disposing);
        }
    }
}
