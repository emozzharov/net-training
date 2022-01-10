/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�TCPThread.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��36��
 * * �ļ���ʶ��795AF74A-746C-4C84-89BE-B98CB50837E2
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
    /// TCP���ӿ�����.
    /// </summary>
    public class TCPThread : BaseClass
    {
        /// <summary>
        /// �������ݵĻ�������С��Ϊ100K.
        /// </summary>
        private const int BufferSize = 1024 * 0xFF;

        /// <summary>
        /// Socket.
        /// </summary>
        public Socket socket;

        /// <summary>
        /// �������ݵĻ�����.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// ָʾSocket�Ƿ�����.
        /// </summary>
        private bool connected;

        /// <summary>
        /// TCPThread���캯��
        /// </summary>
        public TCPThread()
        {
            this.connected = false;
            this.buffer = new byte[BufferSize];
        }

        /// <summary>
        /// TCPThread���캯��������Ĭ��socket����.
        /// </summary>
        /// <param name="sock">socket����.</param>
        public TCPThread(Socket sock)
            : base()
        {
            this.socket = sock;
            this.connected = true;
        }

        /// <summary>
        /// ���ݵ�����¼�����
        /// </summary>
        public event DataArriveEvent OnDataArrive;

        /// <summary>
        /// �����ʳ���
        /// </summary>
        public event Action<int> OnError;

        /// <summary>
        /// �����ӱ��ر�
        /// </summary>
        public event NullParamEvent OnClose;

        /// <summary>
        /// ָʾSocket�Ƿ�����.
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
        /// ָʾSocket���Կ�ʼ��������.
        /// </summary>
        public void BeginReceive()
        {
            this.socket.BeginReceive(this.buffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(this.OnReceive), this.socket);
        }

        /// <summary>
        /// ���Ͷ���������.
        /// </summary>
        /// <param name="data">��Ҫ���͵�����.</param>
        public virtual void Send(byte[] data)
        {
            this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.OnEndSend), this.socket);
        }

        /// <summary>
        /// �����ַ�������.
        /// </summary>
        /// <param name="stringData">��Ҫ���͵�����.</param>
        public virtual void Send(string stringData)
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes(stringData);
            this.Send(data);
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
                this.buffer = null;
            }

            // �ͷŷ��й���Դ
            if (this.connected)
            {
                this.OnCloseEvent();
            }

            this.socket = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// ��socket����.
        /// </summary>
        /// <param name="errNum">������.</param>
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
        /// socket�Ѿ��ر�.
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
        /// ���ݵ����¼�.
        /// </summary>
        /// <param name="tcpThread">��ǰtcp�Ựsocket.</param>
        /// <param name="buffer">����.</param>
        protected void OnDataArriveEvent(TCPThread tcpThread, byte[] buffer)
        {
            if (this.OnDataArrive != null)
            {
                this.OnDataArrive(tcpThread, buffer);
            }
        }

        /// <summary>
        /// ���ݵ�����첽������.
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
        /// �첽�����������.
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
