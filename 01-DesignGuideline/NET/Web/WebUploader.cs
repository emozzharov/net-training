/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�WebUploader.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��58��
 * * �ļ���ʶ��FC0090B0-D61A-4503-9952-5A1C215E3815
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Codest.Net.Web
{
    /// <summary>
    /// ����ʵ�����ļ��ϴ����ܣ���Ҫָ��HtmlInputFile �ؼ�
    /// ����1�����Զ��ļ����ͽ�������
    /// ����2�����Զ��ļ���С���޽�������
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
        private System.Web.UI.HtmlControls.HtmlInputFile scrfile; // HtmlInputFile �ؼ�
        private string savepath = string.Empty; // �����ļ���·��
        private string newfilename = string.Empty; // �ļ�������Ϊ
        private string newextfile = string.Empty; // �ļ���׺
        private int maxsize = 0; // �ļ���С����
        private string extfile = string.Empty; // ����ĺ�׺�����á������ָ������.����Ϊ��ʱ����ȫ���ļ�����

        /// <summary>
        /// ���캯������ָ���κ�����.
        /// </summary>
        public WebUploader()
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="scrFile">HtmlInputFile �ؼ�.</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        {
            this.FileSource = scrFile;
        }

        /// <summary>
        /// ���캯�����ϴ����ļ��������޸�.
        /// </summary>
        /// <param name="scrFile">HtmlInputFile �ؼ�.</param>
        /// <param name="savePath">����·��.</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string savePath)
        {
            this.FileSource = scrFile;
            this.savepath = savePath;
            this.newfilename = scrFile?.PostedFile.FileName;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="scrFile">HtmlInputFile �ؼ�.</param>
        /// <param name="savePath">����·��.</param>
        /// <param name="newFileName">�µ��ļ�������������׺��.</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string savePath, string newFileName)
        {
            this.FileSource = scrFile;
            this.savepath = savePath;
            this.newfilename = newFileName;
        }

        /// <summary>
        /// ��ȡ��ָ���ļ�����·��.
        /// </summary>
        public string SavePath
        {
            get
            {
                return this.savepath;
            }

            set
            {
                this.savepath = value;
                if (this.savepath.Substring(this.savepath.Length) != "\\")
                {
                    this.savepath += "\\";
                }
            }
        }

        /// <summary>
        /// ��ȡ��ָ���ļ���С����.
        /// </summary>
        public int MaxSize
        {
            get { return this.maxsize; }
            set { this.maxsize = value; }
        }

        /// <summary>
        /// ��ȡ��ָ��������ļ���׺�б��á������ָ������.��.
        /// </summary>
        public string AllowExtFile
        {
            get { return this.extfile; }
            set { this.extfile = value; }
        }

        /// <summary>
        /// ��ȡ��ָ���µ��ļ�������������׺.
        /// </summary>
        public string NewFileName
        {
            get { return this.newfilename; }
            set { this.newfilename = value; }
        }

        /// <summary>
        /// ��ȡ��ָ��HtmlInputFile�ؼ�.
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputFile FileSource
        {
            get
            {
                return this.scrfile;
            }

            set
            {
                string s;
                this.scrfile = value;
                s = this.scrfile.PostedFile.FileName;
                s = s.Substring(s.LastIndexOf('.'));
                this.newextfile = s;
            }
        }

        /// <summary>
        /// ׼�������󣬿�ʼ�ϴ�.
        /// </summary>
        /// <returns>some return.</returns>
        public int Start()
        {
            if (this.scrfile.PostedFile.ContentLength == 0)
            {
                return 504; // no source
            }
            else if ((this.scrfile.PostedFile.ContentLength >= this.maxsize) && (this.maxsize != 0))
            {
                return 501; // out of the range
            }
            else if (string.IsNullOrEmpty(this.savepath) || string.IsNullOrEmpty(this.newfilename))
            {
                return 505; // no filename or path
            }
            else if (!this.CheckExt())
            {
                return 502; // ext is not allow.
            }

            try
            {
                this.scrfile.PostedFile.SaveAs(this.savepath + this.newfilename + this.newextfile);
                return 0;
            }
            catch
            {
                return 500; // unknow error
            }
        }

        /// <summary>
        /// ����start()��������ֵ��Ϊ0�����ÿɻ�ȡ������Ϣ.
        /// </summary>
        /// <param name="errCode">some param.</param>
        /// <returns>some return.</returns>
        public string GetErr(int errCode)
        {
            switch (errCode)
            {
                case 500:
                    return "δ֪�ڲ����ⲿ�Ĵ���";
                case 501:
                    return "�ļ���С��������";
                case 502:
                    return "�ļ����Ͳ����Ϲ涨��ֻ����" + this.extfile + "���͵��ļ�";
                case 504:
                    return "û��ָ����Ҫ�ϴ����ļ�";
                default:
                    return "δ֪�ڲ����ⲿ�Ĵ���";
            }
        }

        /// <summary>
        /// ����׺�Ƿ����Ҫ��.
        /// </summary>
        /// <returns>some return.</returns>
        private bool CheckExt()
        {
            if (string.IsNullOrWhiteSpace(this.extfile))
            {
                return true;
            }

            string[] exts = null;
            exts = this.extfile.Split(new char[] { ';' });
            int i = 0;
            for (i = 0; i <= exts.GetUpperBound(0); i++)
            {
                if (exts[i] == this.newextfile)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
