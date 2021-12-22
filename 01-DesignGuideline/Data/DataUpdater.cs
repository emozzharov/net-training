/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DataUpdater.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ00��41��
 * * �ļ���ʶ��15ED7E83-DEB1-44EE-933A-FFB9A126E20D
 * * ����ժҪ��
 * *******************************************************************************/


using System.Data;

namespace Codest.Data
{
    /// <summary>
    /// �������ݿ�������Ļ���
    /// </summary>
    public abstract class DataUpdater : BaseClass
    {
        #region ��Ա����
        private bool autoRelease;
        internal int updaterId;
        #endregion

        #region �ӿڷ�װ
        /// <summary>
        /// ָʾ����Update()���Ƿ��Զ��ͷŵ�ǰ������
        /// Ĭ��Ϊ��true
        /// </summary>
        public bool AutoRelease
        {
            get { return autoRelease; }
            set { autoRelease = value; }
        }
        #endregion

        #region ����/��������
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">����������Ψһ��ID</param>
        public DataUpdater(int id)
        {
            autoRelease = true;
            updaterId = id;
        }
        /// <summary>
        /// ��������
        /// </summary>
        ~DataUpdater()
        {
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
            base.Dispose(disposing);
        }
        #endregion

        #region  protected virtual void DecideRelease()
        /// <summary>
        /// �����Ƿ��Զ��ͷŸ�����
        /// </summary>
        protected virtual void ReleaseDecide()
        {
            if (autoRelease)
                Release();
        }
        #endregion

        #region public abstract DataTable SelectWithUpdate(string SQLCmd)

        /// <summary>
        /// ʹ���󽫽����޸�ģʽ״̬
        /// �û������޸ķ��ؽ��������ɾ�����޸ĺ�������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬
        /// </summary>
        public abstract DataTable SelectWithUpdate(string sqlCommand);
        #endregion

        #region public abstract DataTable InsertMode(string TableName)

        /// <summary>
        /// ʹ��������޸�ģʽ
        /// �û������ڷ��ر�ṹ��DataTable���������
        /// �ٵ���Update(DataTable)���и��²���
        /// �˺󣬶����˳��޸�ģʽ״̬
        /// </summary>
        public abstract DataTable InsertMode(string tableName);
        #endregion

        #region public abstract void Update(System.Data.DataTable DataTableSource)

        /// <summary>
        /// �ر��޸�ģʽ,������DataTable���и��²���
        /// </summary>
        public abstract void Update(DataTable dataTableSource);
        #endregion

        #region  public abstract void Release()
        /// <summary>
        /// �ͷŵ�ǰ������
        /// </summary>
        public abstract void Release();
        #endregion

    }

}
