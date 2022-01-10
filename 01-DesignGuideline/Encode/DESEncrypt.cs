/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：DESEncrypt.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分15秒
 * * 文件标识：29E9467D-FDF9-46F7-A47C-531AEA3718EF
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Codest.Encode
{
    /// <summary>
    /// 实现数据的DES加密解密.
    /// </summary>
    public class DESEncrypt : SerializableBaseClass<DESEncrypt>
    {
        /// <summary>
        /// 固化的加密解密数据.
        /// </summary>
        private byte[] keys;

        /// <summary>
        /// 密钥.
        /// </summary>
        private string encryptKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DESEncrypt()
        {
            this.keys = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptKey">密钥</param>
        public DESEncrypt(string encryptKey)
            : this()
        {
            this.encryptKey = encryptKey;
        }

        /// <summary>
        /// 内部固化加密Key.
        /// </summary>
        public byte[] Keys
        {
            get { return this.keys; }
            set { this.keys = value; }
        }

        /// <summary>
        /// 密钥.
        /// </summary>
        public string EncryptKey
        {
            get { return this.encryptKey; }
            set { this.encryptKey = value; }
        }

        /// <summary>
        /// 加密数据.
        /// </summary>
        /// <param name="srcData">要加密的字符串.</param>
        /// <returns>加密过的数据.</returns>
        public string Encrypt(string srcData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(srcData);
            byte[] outputByteArray = this.Encrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }

        /// <summary>
        /// 加密数据.
        /// </summary>
        /// <param name="srcData">要加密的数据流.</param>
        /// <returns>加密过的数据.</returns>
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
        /// 解密数据.
        /// </summary>
        /// <param name="srcData">需要解密的数据.</param>
        /// <returns>解密后的数据.</returns>
        public string Decrypt(string srcData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(srcData);
            byte[] outputByteArray = this.Decrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }

        /// <summary>
        /// 解密数据.
        /// </summary>
        /// <param name="srcData">需要解密的数据.</param>
        /// <returns>解密后的数据.</returns>
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
