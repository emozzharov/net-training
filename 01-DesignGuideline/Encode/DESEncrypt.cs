/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�DESEncrypt.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ01��15��
 * * �ļ���ʶ��29E9467D-FDF9-46F7-A47C-531AEA3718EF
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Codest.Encode
{
    /// <summary>
    /// ʵ�����ݵ�DES���ܽ���.
    /// </summary>
    public class DESEncrypt : SerializableBaseClass<DESEncrypt>
    {
        /// <summary>
        /// �̻��ļ��ܽ�������.
        /// </summary>
        private byte[] keys;

        /// <summary>
        /// ��Կ.
        /// </summary>
        private string encryptKey;

        /// <summary>
        /// ���캯��
        /// </summary>
        public DESEncrypt()
        {
            this.keys = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="encryptKey">��Կ</param>
        public DESEncrypt(string encryptKey)
            : this()
        {
            this.encryptKey = encryptKey;
        }

        /// <summary>
        /// �ڲ��̻�����Key.
        /// </summary>
        public byte[] Keys
        {
            get { return this.keys; }
            set { this.keys = value; }
        }

        /// <summary>
        /// ��Կ.
        /// </summary>
        public string EncryptKey
        {
            get { return this.encryptKey; }
            set { this.encryptKey = value; }
        }

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="srcData">Ҫ���ܵ��ַ���.</param>
        /// <returns>���ܹ�������.</returns>
        public string Encrypt(string srcData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(srcData);
            byte[] outputByteArray = this.Encrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="srcData">Ҫ���ܵ�������.</param>
        /// <returns>���ܹ�������.</returns>
        public byte[] Encrypt(byte[] srcData)
        {
            try
            {
                byte[] rgbKey = Encoding.ASCII.GetBytes(this.encryptKey.Substring(0, 8));
                byte[] rgbIV = this.keys;
                byte[] inputByteArray = srcData;
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="srcData">��Ҫ���ܵ�����.</param>
        /// <returns>���ܺ������.</returns>
        public string Decrypt(string srcData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(srcData);
            byte[] outputByteArray = this.Decrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="srcData">��Ҫ���ܵ�����.</param>
        /// <returns>���ܺ������.</returns>
        public byte[] Decrypt(byte[] srcData)
        {
            try
            {
                byte[] rgbKey = Encoding.ASCII.GetBytes(this.encryptKey);
                byte[] rgbIV = this.keys;
                byte[] inputByteArray = srcData;
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
    }
}
