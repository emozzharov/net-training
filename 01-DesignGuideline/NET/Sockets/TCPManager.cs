/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：TCPManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分43秒
 * * 文件标识：3A5149DB-F43C-4651-B708-FF9022AFF8FA
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Codest.Net.Sockets
{
    /// <summary>
    /// 数据到达进行处理的函数的委托.
    /// </summary>
    /// <param name="tcpThread">当前tcp会话对象.</param>
    /// <param name="buffer">数据.</param>
    public delegate void DataArriveEvent(TCPThread tcpThread, byte[] buffer);

    /// <summary>
    /// 提供TCP连接服务端的类.
    /// </summary>
    public class TCPManager : BaseClass
    {
        private Socket socket;

        /// <summary>
        /// 客户端数据通知事件.
        /// </summary>
        private DataArriveEvent onClientDataArrive;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public TCPManager()
        {
            this.socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
        }

        /// <summary>
        /// 返回当前Socket.
        /// </summary>
        public Socket Socket
        {
            get { return this.socket; }
        }

        /// <summary>
        /// 获取当前连接的本地TCP端口.
        /// </summary>
        public int LocalPort
        {
            get { return ((IPEndPoint)this.socket.LocalEndPoint).Port; }
        }

        /// <summary>
        /// 创建Socket.
        /// </summary>
        /// <param name="localPort">本地TCP端口.</param>
        /// <returns>创建是否成功.</returns>
        public bool Create(int localPort)
        {
            IPEndPoint ep;
            try
            {
                ep = new IPEndPoint(IPAddress.Any, localPort);
                this.socket.Bind(ep);
                this.socket.Listen(0);
                return true;
            }
            catch
            {
                this.socket.Close();
                return false;
            }
        }

        /// <summary>
        /// 关闭Socket连接.
        /// </summary>
        public void Close()
        {
            this.socket.Close();
        }

        /// <summary>
        /// 指示Socket可以开始处理连接请求了.
        /// </summary>
        /// <returns>调用是否成功.</returns>
        public bool BeginAccept()
        {
            try
            {
                this.socket.BeginAccept(new AsyncCallback(this.OnAccept), this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 指示Socket停止处理连接请求.
        /// </summary>
        /// <returns>调用是否成功.</returns>
        public bool EndAccept()
        {
            try
            {
                this.socket.EndAccept(null);
                return true;
            }
            catch
            {
                return false;
            }
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
            }

            // 释放非托管资源
            this.socket.Close();
            this.socket = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// 当有收到客户端数据时，触发异步回调函数.
        /// </summary>
        /// <param name="tcpThread">some param.</param>
        /// <param name="buffer">another param.</param>
        protected void OnClientDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        {
            if (this.onClientDataArrive != null)
            {
                this.onClientDataArrive(tcpThread, buffer);
            }
        }

        /// <summary>
        /// 当有连接请求接入时，触发异步回调函数.
        /// </summary>
        /// <param name="ar">some param.</param>
        protected void OnAccept(IAsyncResult ar)
        {
            TCPThread tcpthread;
            Socket sock;
            try
            {
                sock = this.socket.EndAccept(ar);
                tcpthread = new TCPThread(sock);
                tcpthread.OnDataArrive += new DataArriveEvent(this.OnClientDataArriveEvent);
                tcpthread.BeginReceive();
            }
            catch
            {
                throw;
            }
            finally
            {
                this.BeginAccept();
            }
        }
    }
}
