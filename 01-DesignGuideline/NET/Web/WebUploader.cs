/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�WebUploader.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��58��
 * * �ļ���ʶ��FC0090B0-D61A-4503-9952-5A1C215E3815
 * * ����ժҪ��
 * *******************************************************************************/
using System.Web.UI.HtmlControls;

namespace Codest.Net.Web
{
    /// <summary>   
    /// ����ʵ�����ļ��ϴ����ܣ���Ҫָ��HtmlInputFile �ؼ�   
    /// ����1�����Զ��ļ����ͽ�������   
    /// ����2�����Զ��ļ���С���޽�������   
    ///    
    /// example:   
    ///  WebUploader up;   
    ///  up = new WebUploader(HtmlInputFile1);   
    ///  up.SvaePath = "c:\\inetpub\\wwwroot\\upload\\"; //����ָ���������ļ���·��   
    ///  up.AllowExtFile = ".jpg;.gif;"; //���������   
    ///  up.MaxSize = 500 * 1024; //��С����500k   
    ///  up.NewFileName = "newfile1"; //ָ���µ��ļ�������ָ�����޸�   
    ///  int errcode = up.Start(); //��ʼ�ϴ�   
    ///  string errmsg = up.GetErr(errcode); //��ô���������Ϣ   
    ///  Response.write(errmsg);��//��ʾ������Ϣ   
    /// </summary>   
    public class WebUploader : BaseClass
    {
        #region ��Ա����
        private HtmlInputFile sourceFile;//HtmlInputFile �ؼ�   
        private string savePath = "";//�����ļ���·��   
        private string newFilename = "";//�ļ�������Ϊ   
        private string newFileExtension = "";//�ļ���׺   
        private int maxSize = 0;//�ļ���С����   
        private string fileExtension = "";//����ĺ�׺�����á������ָ������.����Ϊ��ʱ����ȫ���ļ�����   
        #endregion

        #region �ӿڷ�װ

        #region public string SavePath
        /// <summary>
        /// ��ȡ��ָ���ļ�����·��   
        /// </summary>
        public string SavePath
        {
            get { return savePath; }
            set
            {
                savePath = value;
                if (savePath.Substring(savePath.Length) != "\\")
                {
                    savePath += "\\";
                }
            }
        }
        #endregion public string SavePath

        #region public int MaxSize
        /// <summary>
        /// ��ȡ��ָ���ļ���С����
        /// </summary>
        public int MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }
        #endregion

        #region public string AllowExtFile
        /// <summary>
        /// ��ȡ��ָ��������ļ���׺�б��á������ָ������.��   
        /// </summary>
        public string AllowFileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }
        #endregion

        #region public string NewFileName
        /// <summary>
        /// ��ȡ��ָ���µ��ļ�������������׺   
        /// </summary>
        public string NewFilename
        {
            get { return newFilename; }
            set { newFilename = value; }
        }
        #endregion

        #region public System.Web.UI.HtmlControls.HtmlInputFile FileSource
        /// <summary>
        /// ��ȡ��ָ��HtmlInputFile�ؼ�   
        /// </summary>
        public HtmlInputFile FileSource
        {
            get { return sourceFile; }
            set
            {
                string fileExtension;
                sourceFile = value;
                fileExtension = sourceFile.PostedFile.FileName;
                fileExtension = fileExtension.Substring(fileExtension.LastIndexOf('.'));
                newFileExtension = fileExtension;
            }
        }
        #endregion
        
        #endregion

        #region ����/��������

        #region public WebUploader()
        /// <summary>
        /// ���캯������ָ���κ�����   
        /// </summary>
        public WebUploader()
        {

        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sourceFile">HtmlInputFile �ؼ�</param>
        public WebUploader(HtmlInputFile sourceFile)
        {
            this.FileSource = sourceFile;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath)
        /// <summary>
        /// ���캯�����ϴ����ļ��������޸�   
        /// </summary>
        /// <param name="sourceFile">HtmlInputFile �ؼ�</param>
        /// <param name="savePath">����·��</param>
        public WebUploader(HtmlInputFile sourceFile, string savePath)
        {
            this.FileSource = sourceFile;
            this.savePath = savePath;
            newFilename = sourceFile.PostedFile.FileName;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath, string NewFileName)
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="sourceFile">HtmlInputFile �ؼ�</param>
        /// <param name="savePath">����·��</param>
        /// <param name="newFileName">�µ��ļ�������������׺��</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile sourceFile, string savePath, string newFileName)
        {
            this.FileSource = sourceFile;
            this.savePath = savePath;
            newFilename = newFileName;
        }
        #endregion
        
        #endregion

        #region private bool CheckExt()
        /// <summary>
        /// ����׺�Ƿ����Ҫ��
        /// </summary>
        /// <returns></returns>
        private bool CheckExtension()
        {
            if (fileExtension == "") return true;
            string[] extensions = null;
            extensions = fileExtension.Split(new char[] { ';' });
            int i = 0;
            for (i = 0; i <= extensions.GetUpperBound(0); i++)
            {
                if (extensions[i] == newFileExtension) return true;
            }
            return false;
        }
        #endregion 

        #region public int Start()
        /// <summary>
        /// ׼�������󣬿�ʼ�ϴ�
        /// </summary>
        /// <returns></returns>
        public int Start()
        {
            if (sourceFile.PostedFile.ContentLength == 0)
            {
                return 504; //no source   
            }
            else if ((sourceFile.PostedFile.ContentLength >= maxSize) && (maxSize != 0))
            {
                return 501; //out of the range   
            }
            else if ((savePath == "") || (newFilename == ""))
            {
                return 505; //no filename or path    
            }
            else if (!CheckExtension())
            {
                return 502; //ext is not allow   
            }
            try
            {
                sourceFile.PostedFile.SaveAs(savePath + newFilename + newFileExtension);
                return 0;
            }
            catch
            {
                return 500; //unknow error   
            }
        }
        #endregion

        #region public string GetErr(int errCode)
        /// <summary>
        /// ����start()��������ֵ��Ϊ0�����ÿɻ�ȡ������Ϣ   
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public string GetError(int errorCode)
        {
            switch (errorCode)
            {
                case 500:
                    return "δ֪�ڲ����ⲿ�Ĵ���";
                case 501:
                    return "�ļ���С��������";
                case 502:
                    return "�ļ����Ͳ����Ϲ涨��ֻ����" + fileExtension + "���͵��ļ�";
                case 504:
                    return "û��ָ����Ҫ�ϴ����ļ�";
                default:
                    return "δ֪�ڲ����ⲿ�Ĵ���";
            }
        }
        #endregion
    }  

}
