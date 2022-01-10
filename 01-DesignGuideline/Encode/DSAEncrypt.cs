/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DSAEncrypt.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��16��
 * * �ļ���ʶ��056C74BA-5776-4E24-9BBB-DE22E25481B7
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Codest.Encode;
using Codest.Net;

namespace Codest.Encode
{
    /// <summary>
    /// ʵ�ֶ����ݽ���DSAǩ������֤.
    /// </summary>
    public class DSAEncrypt : SerializableBaseClass<DSAEncrypt>
    {
        /// <summary>
        /// �ṩDSA�㷨.
        /// </summary>
        private DSACryptoServiceProvider dsac;

        /// <summary>
        /// ���캯��.
        /// </summary>
        public DSAEncrypt()
        {
            this.dsac = new DSACryptoServiceProvider();
        }

        /// <summary>
        /// ��������
        /// </summary>
        ~DSAEncrypt()
        {
            this.dsac.Clear();
        }

        /// <summary>
        /// ���ַ������ݽ���ǩ��.
        /// </summary>
        /// <param name="srcData">�ַ�������.</param>
        /// <returns>DSAǩ��.</returns>
        public byte[] GetSignature(string srcData)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(srcData);
            return this.GetSignature(binaryData);
        }

        /// <summary>
        /// �Զ��������ݽ���ǩ��.
        /// </summary>
        /// <param name="srcData">����������.</param>
        /// <returns>DSAǩ��.</returns>
        public byte[] GetSignature(byte[] srcData)
        {
            byte[] sign = this.dsac.SignData(srcData);
            return sign;
        }

        /// <summary>
        /// ��֤ǩ��.
        /// </summary>
        /// <param name="srcData">��Ҫ��֤������.</param>
        /// <param name="signature">DSAǩ��.</param>
        /// <returns>ǩ���Ƿ���ȷ.</returns>
        public bool VerifySignature(byte[] srcData, byte[] signature)
        {
            bool ver = this.dsac.VerifyData(srcData, signature);
            return ver;
        }

        /// <summary>
        /// ��֤ǩ��.
        /// </summary>
        /// <param name="srcData">��Ҫ��֤������.</param>
        /// <param name="signature">DSAǩ��.</param>
        /// <returns>ǩ���Ƿ���ȷ.</returns>
        public bool VerifySignature(string srcData, byte[] signature)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(srcData);
            bool ver = this.dsac.VerifyData(binaryData, signature);
            return ver;
        }
    }
}
