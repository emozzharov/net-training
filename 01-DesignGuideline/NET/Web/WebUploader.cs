/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：WebUploader.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分58秒
 * * 文件标识：FC0090B0-D61A-4503-9952-5A1C215E3815
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Codest.Net.Web
{
    /// <summary>
    /// 该类实现了文件上传功能，需要指定HtmlInputFile 控件
    /// 功能1：可以对文件类型进行限制
    /// 功能2：可以对文件大小上限进行限制
    /// example:
    ///  WebUploader up;
    ///  up = new WebUploader(HtmlInputFile1);
    ///  up.SvaePath = "c:\\inetpub\\wwwroot\\upload\\"; //必须指定，保存文件的路径
    ///  up.AllowExtFile = ".jpg;.gif;"; //允许的类型
    ///  up.MaxSize = 500 * 1024; //大小限制500k
    ///  up.NewFileName = "newfile1"; //指定新的文件名，不指定则不修改
    ///  int errcode = up.Start(); //开始上传
    ///  string errmsg = up.GetErr(errcode); //获得错误描述信息
    ///  Response.write(errmsg);　//显示错误信息
    /// </summary>
    public class WebUploader : BaseClass
    {
        private System.Web.UI.HtmlControls.HtmlInputFile scrfile; // HtmlInputFile 控件
        private string savepath = string.Empty; // 保存文件的路径
        private string newfilename = string.Empty; // 文件重命名为
        private string newextfile = string.Empty; // 文件后缀
        private int maxsize = 0; // 文件大小限制
        private string extfile = string.Empty; // 允许的后缀名，用“；”分割，包含“.”，为空时允许全部文件类型

        /// <summary>
        /// 构造函数，不指定任何数据.
        /// </summary>
        public WebUploader()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scrFile">HtmlInputFile 控件.</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        {
            this.FileSource = scrFile;
        }

        /// <summary>
        /// 构造函数，上传后文件名不作修改.
        /// </summary>
        /// <param name="scrFile">HtmlInputFile 控件.</param>
        /// <param name="savePath">保存路径.</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string savePath)
        {
            this.FileSource = scrFile;
            this.savepath = savePath;
            this.newfilename = scrFile?.PostedFile.FileName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scrFile">HtmlInputFile 控件.</param>
        /// <param name="savePath">保存路径.</param>
        /// <param name="newFileName">新的文件名（不包含后缀）.</param>
        public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string savePath, string newFileName)
        {
            this.FileSource = scrFile;
            this.savepath = savePath;
            this.newfilename = newFileName;
        }

        /// <summary>
        /// 获取或指定文件保存路径.
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
        /// 获取或指定文件大小上限.
        /// </summary>
        public int MaxSize
        {
            get { return this.maxsize; }
            set { this.maxsize = value; }
        }

        /// <summary>
        /// 获取或指定允许的文件后缀列表，用“；”分割，包含“.”.
        /// </summary>
        public string AllowExtFile
        {
            get { return this.extfile; }
            set { this.extfile = value; }
        }

        /// <summary>
        /// 获取或指定新的文件名，不包含后缀.
        /// </summary>
        public string NewFileName
        {
            get { return this.newfilename; }
            set { this.newfilename = value; }
        }

        /// <summary>
        /// 获取或指定HtmlInputFile控件.
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
        /// 准备就绪后，开始上传.
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
        /// 调用start()后，若返回值不为0，调用可获取错误信息.
        /// </summary>
        /// <param name="errCode">some param.</param>
        /// <returns>some return.</returns>
        public string GetErr(int errCode)
        {
            switch (errCode)
            {
                case 500:
                    return "未知内部或外部的错误";
                case 501:
                    return "文件大小超出限制";
                case 502:
                    return "文件类型不符合规定，只允许：" + this.extfile + "类型的文件";
                case 504:
                    return "没有指定需要上传的文件";
                default:
                    return "未知内部或外部的错误";
            }
        }

        /// <summary>
        /// 检测后缀是否符合要求.
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
