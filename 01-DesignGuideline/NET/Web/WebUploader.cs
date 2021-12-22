/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：WebUploader.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分58秒
 * * 文件标识：FC0090B0-D61A-4503-9952-5A1C215E3815
 * * 内容摘要：
 * *******************************************************************************/
using System.Web.UI.HtmlControls;

namespace Codest.Net.Web
{
    /// <summary>   
    /// 该类实现了文件上传功能，需要指定HtmlInputFile 控件   
    /// 功能1：可以对文件类型进行限制   
    /// 功能2：可以对文件大小上限进行限制   
    ///    
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
        #region 成员变量
        private HtmlInputFile sourceFile;//HtmlInputFile 控件   
        private string savePath = "";//保存文件的路径   
        private string newFilename = "";//文件重命名为   
        private string newFileExtension = "";//文件后缀   
        private int maxSize = 0;//文件大小限制   
        private string fileExtension = "";//允许的后缀名，用“；”分割，包含“.”，为空时允许全部文件类型   
        #endregion

        #region 接口封装

        #region public string SavePath
        /// <summary>
        /// 获取或指定文件保存路径   
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
        /// 获取或指定文件大小上限
        /// </summary>
        public int MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }
        #endregion

        #region public string AllowExtFile
        /// <summary>
        /// 获取或指定允许的文件后缀列表，用“；”分割，包含“.”   
        /// </summary>
        public string AllowFileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }
        #endregion

        #region public string NewFileName
        /// <summary>
        /// 获取或指定新的文件名，不包含后缀   
        /// </summary>
        public string NewFilename
        {
            get { return newFilename; }
            set { newFilename = value; }
        }
        #endregion

        #region public System.Web.UI.HtmlControls.HtmlInputFile FileSource
        /// <summary>
        /// 获取或指定HtmlInputFile控件   
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

        #region 构造/析构函数

        #region public WebUploader()
        /// <summary>
        /// 构造函数，不指定任何数据   
        /// </summary>
        public WebUploader()
        {

        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile)
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceFile">HtmlInputFile 控件</param>
        public WebUploader(HtmlInputFile sourceFile)
        {
            this.FileSource = sourceFile;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath)
        /// <summary>
        /// 构造函数，上传后文件名不作修改   
        /// </summary>
        /// <param name="sourceFile">HtmlInputFile 控件</param>
        /// <param name="savePath">保存路径</param>
        public WebUploader(HtmlInputFile sourceFile, string savePath)
        {
            this.FileSource = sourceFile;
            this.savePath = savePath;
            newFilename = sourceFile.PostedFile.FileName;
        }
        #endregion

        #region public WebUploader(System.Web.UI.HtmlControls.HtmlInputFile scrFile, string SavePath, string NewFileName)
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceFile">HtmlInputFile 控件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="newFileName">新的文件名（不包含后缀）</param>
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
        /// 检测后缀是否符合要求
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
        /// 准备就绪后，开始上传
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
        /// 调用start()后，若返回值不为0，调用可获取错误信息   
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public string GetError(int errorCode)
        {
            switch (errorCode)
            {
                case 500:
                    return "未知内部或外部的错误";
                case 501:
                    return "文件大小超出限制";
                case 502:
                    return "文件类型不符合规定，只允许：" + fileExtension + "类型的文件";
                case 504:
                    return "没有指定需要上传的文件";
                default:
                    return "未知内部或外部的错误";
            }
        }
        #endregion
    }  

}
