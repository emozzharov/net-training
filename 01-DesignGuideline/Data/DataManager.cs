/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DataManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��43��
 * * �ļ���ʶ��124F76B9-05EE-4B2A-9400-A233BC1D4BF3
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// �������ݿ�������Ļ���.
    /// </summary>
    public abstract class DataManager : BaseClass
    {
        private int execNum = 0;

        /// <summary>
        /// ���浱ǰ�������������ѷ���ĸ�����.
        /// </summary>
        private Hashtable dataUpdaterColl;
        private string connString = string.Empty;
        private string dbscr = string.Empty;

        /// <summary>
        /// ���캯��
        /// </summary>
        protected DataManager()
        {
            this.dataUpdaterColl = new Hashtable();
        }

        /// <summary>
        /// ��ȡ���������ݿ������ַ���.
        /// </summary>
        public string ConnString
        {
          get { return this.connString; }
          set { this.connString = value; }
        }

        /// <summary>
        /// ��ȡ��ǰ���ݿ����Ӳ�ѯ�Ĵ���.
        /// </summary>
        public int ExecNum
        {
            get { return this.execNum; }
            set { this.execNum = value; }
        }

        /// <summary>
        /// ��ȡ��ָ�����ݿ�Դ.
        /// </summary>
        public string DataSource
        {
            get { return this.dbscr; }
            set { this.dbscr = value; }
        }

        /// <summary>
        /// Gets dataupdatercoll.
        /// </summary>
        private protected Hashtable DataUpdaterColl => this.dataUpdaterColl;

        /// <summary>
        /// �����ݿ�.
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// ʹ�����ݿ������ַ��������ݿ�.
        /// </summary>
        public abstract void OpenByConnString();

        /// <summary>
        /// �ر�SQL����.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// ִ��SQL���.
        /// </summary>
        /// <param name="sQLCmd">SQL���.</param>
        /// <returns>����Ӧ������.</returns>
        public abstract int Exec(string sQLCmd);

        /// <summary>
        /// ִ��SQL��䣬����Ӧ��������䵽DataTable�У����ܽ��и��²���.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>some return.</returns>
        public abstract DataTable SelectDataTable(string sQLCmd);

        /// <summary>
        /// ѡ��һ����Χ��¼��Select���.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <param name="srcTalbe">srcTable param.</param>
        /// <param name="startRecord">startRecord param.</param>
        /// <param name="maxRecord">max record param.</param>
        /// <returns>some return.</returns>
        public abstract DataTable SelectDataTable(string sQLCmd, string srcTalbe, int startRecord, int maxRecord);

        /// <summary>
        /// ʵ�ַ�ҳ��Select.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <param name="srcTalbe">srcTable param.</param>
        /// <param name="pageSize">startRecord param.</param>
        /// <param name="pageID">max record param.</param>
        /// <returns>some return.</returns>
        public abstract DataTable SelectPage(string sQLCmd, string srcTalbe, int pageSize, int pageID);

        /// <summary>
        /// ִ��ɾ��������SQL���.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>some return.</returns>
        public abstract bool Delete(string sQLCmd);

        /// <summary>
        /// ����һ��������.
        /// </summary>
        /// <returns>some return.</returns>
        public abstract DataUpdater AllocateDataUpdater();

        /// <summary>
        /// �ͷŵ�ǰʵ�������еĸ�����.
        /// </summary>
        public virtual void ReleaseAllDataUpdater()
        {
            foreach (DictionaryEntry de in this.dataUpdaterColl)
            {
                DataUpdater updater = (DataUpdater)de.Value;
                updater.Dispose();
            }

            this.dataUpdaterColl.Clear();
        }

        /// <summary>
        /// �ͷ�һ��������.
        /// </summary>
        /// <param name="updater">������.</param>
        public virtual void ReleaseDataUpdater(DataUpdater updater)
        {
            this.dataUpdaterColl.Remove(updater?.UpdaterID);
            updater.Dispose();
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
            base.Dispose(disposing);
        }
    }
}
