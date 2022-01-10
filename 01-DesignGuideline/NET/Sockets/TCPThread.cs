/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：TCPThread.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分36秒
 * * 文件标识：795AF74A-746C-4C84-89BE-B98CB50837E2
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
    /// TCP连接控制类.
    /// </summary>
    public class TCPThread : BaseClass
    {
        /// <summary>
        /// 接收数据的缓冲区大小，为100K.
        /// </summary>
        private const int BufferSize = 1024 * 0xFF;

        /// <summary>
        /// Socket.
        /// </summary>
        public Socket socket;

        /// <summary>
        /// 接收数据的缓冲区.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// 指示Socket是否连接.
        /// </summary>
        private bool connected;

        /// <summary>
        /// TCPThread构造函数
        /// </summary>
        public TCPThread()
        {
            this.connected = false;
            this.buffer = new byte[BufferSize];
        }

        /// <summary>
        /// TCPThread构造函数，传入默认socket连接.
        /// </summary>
        /// <param name="sock">socket连接.</param>
        public TCPThread(Socket sock)
            : base()
        {
            this.socket = sock;
            this.connected = true;
        }

        /// <summary>
        /// 数据到达的事件处理
        /// </summary>
        public event DataArriveEvent OnDataArrive;

        /// <summary>
        /// 当访问出错
        /// </summary>
        public event Action<int> OnError;

        /// <summary>
        /// 当连接被关闭
        /// </summary>
        public event NullParamEvent OnClose;

        /// <summary>
        /// 指示Socket是否连接.
        /// </summary>
        public bool Connected
        {
            get { return this.connected; }
        }

        /// <summary>
        /// Socket.
        /// </summary>
        public Socket Socket
        {
            get { return this.socket; }
        }

        /// <summary>
        /// 指示Socket可以开始接收数据.
        /// </summary>
        public void BeginReceive()
        {
            this.socket.BeginReceive(this.buffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(this.OnReceive), this.socket);
        }

        /// <summary>
        /// 发送二进制数据.
        /// </summary>
        /// <param name="data">需要发送的数据.</param>
        public virtual void Send(byte[] data)
        {
            this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.OnEndSend), this.socket);
        }

        /// <summary>
        /// 发送字符串数据.
        /// </summary>
        /// <param name="stringData">需要发送的数据.</param>
        public virtual void Send(string stringData)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(stringData);
            this.Send(data);
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
                this.buffer = null;
            }

            // 释放非托管资源
            if (this.connected)
            {
                this.OnCloseEvent();
            }

            this.socket = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// 当socket出错.
        /// </summary>
        /// <param name="errNum">错误编号.</param>
        protected void OnErrorEvent(int errNum)
        {
            this.connected = false;
            this.socket.Close();
            if (this.OnError != null)
            {
                this.OnError(errNum);
            }
        }

        /// <summary>
        /// socket已经关闭.
        /// </summary>
        protected void OnCloseEvent()
        {
            this.connected = false;
            this.socket.Close();
            if (this.OnClose != null)
            {
                this.OnClose();
            }
        }

        /// <summary>
        /// 数据到达事件.
        /// </summary>
        /// <param name="tcpThread">当前tcp会话socket.</param>
        /// <param name="buffer">数据.</param>
        protected void OnDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        {
            if (this.OnDataArrive != null)
            {
                this.OnDataArrive(tcpThread, buffer);
            }
        }

        /// <summary>
        /// 数据到达的异步处理函数.
        /// </summary>
        /// <param name="ar">some params.</param>
        protected void OnReceive(IAsyncResult ar)
        {
            int len;
            try
            {
                len = this.socket.EndReceive(ar);
            }
            catch (SocketException ex)
            {
                this.OnErrorEvent(ex.ErrorCode);
                return;
            }

            if (len == 0)
            {
                this.OnCloseEvent();
            }

            byte[] data = new byte[len];
            Array.Copy(this.buffer, 0, data, 0, len);
            this.OnDataArriveEvent(this, data);
            this.BeginReceive();
        }

        /// <summary>
        /// 异步发送数据完成.
        /// </summary>
        /// <param name="ar">some param.</param>
        protected void OnEndSend(IAsyncResult ar)
        {
            try
            {
                this.socket.EndSend(ar);
            }
            catch (SocketException ex)
            {
                this.OnErrorEvent(ex.ErrorCode);
            }
        }
    }
}
