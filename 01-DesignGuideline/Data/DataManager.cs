/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DataManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��43��
 * * �ļ���ʶ��124F76B9-05EE-4B2A-9400-A233BC1D4BF3
 * * ����ժҪ��
 * *******************************************************************************/

using System.Data;
using System.Collections;

namespace Codest.Data
{
    /// <summary>
    /// �������ݿ�������Ļ���
    /// </summary>
    public abstract class DataManager : BaseClass
    {
        #region ��Ա����
        internal int executionNumber = 0;
        /// <summary>
        /// ���浱ǰ�������������ѷ���ĸ�����
        /// </summary>
        protected Hashtable dataUpdaterCollection;
        private string connectionString = string.Empty;
        private string dbSource = string.Empty;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ��ȡ���������ݿ������ַ���
        /// </summary>
        public string ConnectionString
        {
          get { return connectionString; }
          set { connectionString = value; }
        }
        /// <summary>
        /// ��ȡ��ǰ���ݿ����Ӳ�ѯ�Ĵ���
        /// </summary>
        public int ExecutionNumber
        {
            get { return executionNumber; }
        }

        /// <summary>
        /// ��ȡ��ָ�����ݿ�Դ
        /// </summary>
        public string DataSource
        {
            get { return dbSource; }
            set { dbSource = value; }
        }
        #endregion
         
        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        public DataManager()
        {
            dataUpdaterCollection = new Hashtable();
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~DataManager()
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
            base.Dispose(disposing);
        }
        #endregion

        //--begin--�������ݿ����--

        #region public abstract void Open()
        /// <summary>
        /// �����ݿ�
        /// </summary>
        public abstract void Open();
        #endregion

        #region public abstract void OpenByConnString()
        /// <summary>
        /// ʹ�����ݿ������ַ��������ݿ�
        /// </summary>
        public abstract void OpenByConnectionString();
        #endregion

        #region public abstract void Close()
        /// <summary>
        /// �ر�SQL����
        /// </summary>
        public abstract void Close();
        #endregion

        //--end----�������ݿ����--

        //--begin--�������ݿ����--

        #region public abstract int Exec(string SQLCmd)
        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="sqlCommand">SQL���</param>
        /// <returns>����Ӧ������</returns>
        public abstract int Execute(string sqlCommand);
        #endregion

        #region public abstract DataTable Select(string SQLCmd)

        /// <summary>
        /// ִ��SQL��䣬����Ӧ��������䵽DataTable�У����ܽ��и��²���
        /// </summary>
        public abstract DataTable Select(string sqlCommand);
        #endregion

        #region public abstract DataTable Select(string SQLCmd, string srcTalbe, int startRecord, int maxRecord)

        /// <summary>
        /// ѡ��һ����Χ��¼��Select���
        /// </summary>
        public abstract DataTable Select(string sqlCommand, string sourceTable, int startRecord, int maxRecords);
        #endregion

        #region public abstract DataTable SelectPage(string SQLCmd, string srcTalbe, int pageSize, int pageID)
        /// <summary>
        /// ʵ�ַ�ҳ��Select
        /// </summary>
        public abstract DataTable SelectPage(string sqlCommand, string sourceTable, int pageSize, int pageId);
        #endregion

        #region public abstract bool Delete(string SQLCmd)
        /// <summary>
        /// ִ��ɾ��������SQL���
        /// </summary>
        public abstract bool Delete(string sqlCommand);
        #endregion

        //--end----�������ݿ����--

        //--begin--����������--

        #region public abstract DataUpdater AllocateDataUpdater()
        /// <summary>
        /// ����һ��������
        /// </summary>
        /// <returns></returns>
        public abstract DataUpdater AllocateDataUpdater();
        #endregion

        #region  public virtual void ReleaseAllDataUpdater()
        /// <summary>
        /// �ͷŵ�ǰʵ�������еĸ�����
        /// </summary>
        public virtual void ReleaseAllDataUpdaters()
        {
            foreach (DictionaryEntry entry in dataUpdaterCollection)
            {
                DataUpdater dataUpdater = (DataUpdater)entry.Value;
                dataUpdater.Dispose();
            }
            dataUpdaterCollection.Clear();
        }
        #endregion

        #region  public virtual void ReleaseDataUpdater(DataUpdater updater)
        /// <summary>
        /// �ͷ�һ��������
        /// </summary>
        /// <param name="updater">������</param>
        public virtual void ReleaseDataUpdater(DataUpdater updater)
        {
            this.dataUpdaterCollection.Remove(updater.updaterId);
            updater.Dispose();
        }
        #endregion

        //--end----����������--
    }
}
