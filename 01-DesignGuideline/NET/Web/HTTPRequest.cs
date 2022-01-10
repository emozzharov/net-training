/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：HTTPRequest.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分56秒
 * * 文件标识：3CC6D7ED-740B-4A21-A5E6-1B6C7BF3A9F2
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Codest.Net.Web
{
    /// <summary>
    /// 发起HTTP请求并获得响应结果的类.
    /// </summary>
    public class HTTPRequest : BaseClass
    {
        /// <summary>
        /// 保存当前连接的HTTP Cookies.
        /// </summary>
        private CookieContainer currentCookies;
        private string domainURL;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public HTTPRequest()
        {
            this.currentCookies = new CookieContainer();
        }

        /// <summary>
        /// 构造，并设置网站根路径
        /// 如：http://www.easybroad.com
        /// 最后不需要“/”.
        /// </summary>
        /// <param name="domainURL">网站根路径.</param>
        public HTTPRequest(string domainURL)
        {
            this.currentCookies = new CookieContainer();
            this.domainURL = domainURL;
        }

        /// <summary>
        /// 网站根路径，如：http://www.easybroad.com，最后不需要“/”.
        /// </summary>
        public string DomainURL
        {
            get { return this.domainURL; }
            set { this.domainURL = value; }
        }

        /// <summary>
        /// 断开会话，刷新当前Session及Cookies.
        /// </summary>
        public void RefreshCookies()
        {
            this.currentCookies = new CookieContainer();
        }

        /// <summary>
        /// 使用 HTTP_GET 方法获取数据.
        /// </summary>
        /// <param name="path">相对路径，如“/index.aspx”.</param>
        /// <returns>HTTP响应结果.</returns>
        public string GetData(string path)
        {
            string result;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(this.GetFullURL(path));
            httpRequest.CookieContainer = this.currentCookies;

            WebResponse webResponse = httpRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
            StreamReader readStream = new StreamReader(stream, encode);
            result = readStream.ReadToEnd();
            readStream.Close();
            stream.Close();

            return result;
        }

        /// <summary>
        /// 使用 HTTP_POST 方法获取数据.
        /// </summary>
        /// <param name="path">相对路径，如“/login.aspx”.</param>
        /// <param name="data">需要POST的数据.</param>
        /// <returns>HTTP响应结果.</returns>
        public string PostData(string path, string data)
        {
            string result;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(this.GetFullURL(path));
            httpRequest.CookieContainer = this.currentCookies;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.ContentLength = data.Length;

            // httpRequest.Referer = GetURL("/cn");
            httpRequest.ServicePoint.Expect100Continue = false;

            StreamWriter streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(data);
            streamWriter.Flush();
            streamWriter.Close();
            WebResponse webResponse = httpRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
            StreamReader readStream = new StreamReader(stream, encode);
            result = readStream.ReadToEnd();
            readStream.Close();
            stream.Close();

            return result;
        }

        /// <summary>
        /// 释放由当前对象控制的所有资源.
        /// </summary>
        /// <param name="disposing">显式调用.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // 释放托管资源
                this.currentCookies = null;
                this.domainURL = null;
            }

            // 释放非托管资源
            base.Dispose(disposing);
        }

        /// <summary>
        /// 根据相对路径获取完整的URL.
        /// </summary>
        /// <param name="path">相对路径，如“/login.aspx”.</param>
        /// <returns>完整的URL.</returns>
        protected string GetFullURL(string path)
        {
            return this.DomainURL + path;
        }
    }
}
