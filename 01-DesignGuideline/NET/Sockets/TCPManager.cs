/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�TCPManager.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��43��
 * * �ļ���ʶ��3A5149DB-F43C-4651-B708-FF9022AFF8FA
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
    /// ���ݵ�����д���ĺ�����ί��.
    /// </summary>
    /// <param name="tcpThread">��ǰtcp�Ự����.</param>
    /// <param name="buffer">����.</param>
    public delegate void DataArriveEvent(TCPThread tcpThread, byte[] buffer);

    /// <summary>
    /// �ṩTCP���ӷ���˵���.
    /// </summary>
    public class TCPManager : BaseClass
    {
        private Socket socket;

        /// <summary>
        /// �ͻ�������֪ͨ�¼�.
        /// </summary>
        private DataArriveEvent onClientDataArrive;

        /// <summary>
        /// ���캯��.
        /// </summary>
        public TCPManager()
        {
            this.socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
        }

        /// <summary>
        /// ���ص�ǰSocket.
        /// </summary>
        public Socket Socket
        {
            get { return this.socket; }
        }

        /// <summary>
        /// ��ȡ��ǰ���ӵı���TCP�˿�.
        /// </summary>
        public int LocalPort
        {
            get { return ((IPEndPoint)this.socket.LocalEndPoint).Port; }
        }

        /// <summary>
        /// ����Socket.
        /// </summary>
        /// <param name="localPort">����TCP�˿�.</param>
        /// <returns>�����Ƿ�ɹ�.</returns>
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
        /// �ر�Socket����.
        /// </summary>
        public void Close()
        {
            this.socket.Close();
        }

        /// <summary>
        /// ָʾSocket���Կ�ʼ��������������.
        /// </summary>
        /// <returns>�����Ƿ�ɹ�.</returns>
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
        /// ָʾSocketֹͣ������������.
        /// </summary>
        /// <returns>�����Ƿ�ɹ�.</returns>
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
            this.socket.Close();
            this.socket = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// �����յ��ͻ�������ʱ�������첽�ص�����.
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
        /// ���������������ʱ�������첽�ص�����.
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
