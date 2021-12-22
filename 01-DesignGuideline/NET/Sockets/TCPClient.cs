/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�TCPClient.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��41��
 * * �ļ���ʶ��1126C195-03C7-4B46-9EC7-804837049ACE
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Codest.Net.Sockets
{
    /// <summary>
    /// TCP�ͻ���.
    /// </summary>
    public class TCPClient : TCPThread
    {
        /// <summary>
        /// ���캯��
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
        /// ����������ӳɹ�
        /// </summary>
        public event Action<bool> OnConnect;

        /// <summary>
        /// ���ذ󶨵Ķ˿�.
        /// </summary>
        private int LocalPort
        {
            get { return ((IPEndPoint)this.socket.LocalEndPoint).Port; }
        }

        /// <summary>
        /// ���ӵ�Զ�̶˿�.
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
        /// ���ӵ�Զ��IP��ַ.
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
        /// ͨ��IP����������.
        /// </summary>
        /// <param name="ipAddress">some ip.</param>
        /// <returns>some return.</returns>
        public static IPHostEntry GetHostNameByIP(string ipAddress)
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(ipAddress);
            return hostInfo;
        }

        /// <summary>
        /// ͨ������������IP��ַ.
        /// </summary>
        /// <param name="hostName">some host.</param>
        /// <returns>some return.</returns>
        public static IPAddress GetIPByHostName(string hostName)
        {
            IPAddress[] addrList = Dns.GetHostAddresses(hostName);
            return addrList[0];
        }

        /// <summary>
        /// ����Զ�̷�����.
        /// </summary>
        /// <param name="remoteAddress">��������ַ.</param>
        /// <param name="remotePort">�������˿�.</param>
        public void Connect(string remoteAddress, int remotePort)
        {
            IPEndPoint remoteEP = new IPEndPoint(GetIPByHostName(remoteAddress), remotePort);
            this.socket.BeginConnect(remoteEP, new AsyncCallback(this.EndConnect), this.socket);
        }

        /// <summary>
        /// ����socket.
        /// </summary>
        /// <param name="localPort">���ض˿�.</param>
        /// <returns>�Ƿ�ɹ�.</returns>
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
        /// �ͷ��ɵ�ǰ������Ƶ�������Դ.
        /// </summary>
        /// <param name="disposing">��ʽ����.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // �ͷ��й���Դ
            }

            // �ͷŷ��й���Դ
            base.Dispose(disposing);
        }

        /// <summary>
        /// Connect�ص�.
        /// </summary>
        /// <param name="flag">�����Ƿ�ɹ�.</param>
        protected void OnConnectEvent(bool flag)
        {
            if (this.OnConnect != null)
            {
                this.OnConnect(flag);
            }
        }

        /// <summary>
        /// �첽Connect����.
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
