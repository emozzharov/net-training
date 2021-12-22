/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：TCPClient.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分41秒
 * * 文件标识：1126C195-03C7-4B46-9EC7-804837049ACE
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
    /// TCP客户端.
    /// </summary>
    public class TCPClient : TCPThread
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TCPClient()
            : base()
        {
            this.socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
                );
        }

        /// <summary>
        /// 当请求的连接成功
        /// </summary>
        public event Action<bool> OnConnect;

        /// <summary>
        /// 本地绑定的端口.
        /// </summary>
        private int LocalPort
        {
            get { return ((IPEndPoint)this.socket.LocalEndPoint).Port; }
        }

        /// <summary>
        /// 连接的远程端口.
        /// </summary>
        private int RemotePort
        {
            get
            {
                if (this.Connected)
                {
                    return ((IPEndPoint)this.socket.RemoteEndPoint).Port;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 连接的远程IP地址.
        /// </summary>
        private string RemoteIP
        {
            get
            {
                if (this.Connected)
                {
                    return ((IPEndPoint)this.socket.RemoteEndPoint).Address.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 通过IP查找主机名.
        /// </summary>
        /// <param name="ipAddress">some ip.</param>
        /// <returns>some return.</returns>
        public static IPHostEntry GetHostNameByIP(string ipAddress)
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(ipAddress);
            return hostInfo;
        }

        /// <summary>
        /// 通过主机名查找IP地址.
        /// </summary>
        /// <param name="hostName">some host.</param>
        /// <returns>some return.</returns>
        public static IPAddress GetIPByHostName(string hostName)
        {
            IPAddress[] addrList = Dns.GetHostAddresses(hostName);
            return addrList[0];
        }

        /// <summary>
        /// 连接远程服务器.
        /// </summary>
        /// <param name="remoteAddress">服务器地址.</param>
        /// <param name="remotePort">服务器端口.</param>
        public void Connect(string remoteAddress, int remotePort)
        {
            IPEndPoint remoteEP = new IPEndPoint(GetIPByHostName(remoteAddress), remotePort);
            this.socket.BeginConnect(remoteEP, new AsyncCallback(this.EndConnect), this.socket);
        }

        /// <summary>
        /// 创建socket.
        /// </summary>
        /// <param name="localPort">本地端口.</param>
        /// <returns>是否成功.</returns>
        public bool Create(int localPort)
        {
            IPEndPoint ep;
            try
            {
                ep = new IPEndPoint(IPAddress.Any, this.LocalPort);
                this.socket.Bind(ep);
                return true;
            }
            catch
            {
                this.socket.Close();
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
            base.Dispose(disposing);
        }

        /// <summary>
        /// Connect回调.
        /// </summary>
        /// <param name="flag">连接是否成功.</param>
        protected void OnConnectEvent(bool flag)
        {
            if (this.OnConnect != null)
            {
                this.OnConnect(flag);
            }
        }

        /// <summary>
        /// 异步Connect结束.
        /// </summary>
        /// <param name="ar">some param.</param>
        protected void EndConnect(IAsyncResult ar)
        {
            try
            {
                this.socket.EndConnect(ar);
                this.OnConnectEvent(true);
            }
            catch (SocketException ex)
            {
                this.OnConnectEvent(false);
                this.OnErrorEvent(ex.ErrorCode);
            }
        }
    }
}
