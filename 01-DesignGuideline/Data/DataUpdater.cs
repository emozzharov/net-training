/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DataUpdater.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��41��
 * * �ļ���ʶ��15ED7E83-DEB1-44EE-933A-FFB9A126E20D
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// �������ݿ�������Ļ���.
    /// </summary>
    public abstract class DataUpdater : BaseClass
    {
        private int updaterID;
        private bool autoRelease;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">����������Ψһ��ID.</param>
        protected DataUpdater(int id)
        {
            this.autoRelease = true;
            this.updaterID = id;
        }

        /// <summary>
        /// ָʾ����Update()���Ƿ��Զ��ͷŵ�ǰ������
        /// Ĭ��Ϊ��true.
        /// </summary>
        public bool AutoRelease
        {
            get { return this.autoRelease; }
            set { this.autoRelease = value; }
        }

        /// <summary>
        /// Gets or sets updaterID.
        /// </summary>
        /// <value>updaterID.</value>
        internal int UpdaterID
        {
            get { return this.updaterID; }
            set { this.updaterID = value; }
        }

        /// <summary>
        /// ʹ���󽫽����޸�ģʽ״̬
        /// �û������޸ķ��ؽ��������ɾ�����޸ĺ�������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>something.</returns>
        public abstract DataTable SelectWithUpdate(string sQLCmd);

        /// <summary>
        /// ʹ��������޸�ģʽ
        /// �û������ڷ��ر�ṹ��DataTable���������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬.
        /// </summary>
        /// <param name="tableName">some param.</param>
        /// <returns>something.</returns>
        public abstract DataTable InsertMode(string tableName);

        /// <summary>
        /// �ر��޸�ģʽ,������DataTable���и��²���.
        /// </summary>
        /// <param name="dataTableSource">some param.</param>
        public abstract void Update(System.Data.DataTable dataTableSource);

        /// <summary>
        /// �ͷŵ�ǰ������.
        /// </summary>
        public abstract void Release();

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
            base.Dispose(disposing);
        }

        /// <summary>
        /// �����Ƿ��Զ��ͷŸ�����.
        /// </summary>
        protected virtual void DecideRelease()
        {
            if (this.autoRelease)
            {
                this.Release();
            }
        }
    }
}
