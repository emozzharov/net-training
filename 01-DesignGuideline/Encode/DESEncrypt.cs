/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：DESEncrypt.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分15秒
 * * 文件标识：29E9467D-FDF9-46F7-A47C-531AEA3718EF
 * * 内容摘要：
 * *******************************************************************************/


using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Codest.Encode
{
    /// <summary>
    /// 实现数据的DES加密解密
    /// </summary>
    public class DESEncrypt : SerializableBaseClass<DESEncrypt>
    {
        #region 成员变量
        /// <summary>
        /// 固化的加密解密数据
        /// </summary>
        private byte[] keys;
        /// <summary>
        /// 密钥
        /// </summary>
        private string encryptKey;
        #endregion

        #region 接口封装
        /// <summary>
        /// 内部固化加密Key
        /// </summary>
        public byte[] Keys
        {
            get { return keys; }
            set { keys = value; }
        }
        /// <summary>
        /// 密钥
        /// </summary>
        public string EncryptKey
        {
            get { return encryptKey; }
            set { encryptKey = value; }
        }
        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DESEncrypt()
        {
            keys = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptKey">密钥</param>
        public DESEncrypt(string encryptKey)
            :this()
        {
            this.encryptKey = encryptKey;
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~DESEncrypt()
        { }
        #endregion

        #region public string Encrypt(string srcData)
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="sourceData">要加密的字符串</param>
        /// <returns>加密过的数据</returns>
        public string Encrypt(string sourceData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(sourceData);
            byte[] outputByteArray = Encrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }
        #endregion

        #region public byte[] Encrypt(byte[] srcData)
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="sourceData">要加密的数据流</param>
        /// <returns>加密过的数据</returns>
        public byte[] Encrypt(byte[] sourceData)
        {
            try
            {
                byte[] rgbKey = Encoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = keys;
                byte[] inputByteArray = sourceData;
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region public string Decrypt(string srcData)
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="sourceData">需要解密的数据</param>
        /// <returns>解密后的数据</returns>
        public string Decrypt(string sourceData)
        {
            byte[] inputByteArray = Encoding.ASCII.GetBytes(sourceData);
            byte[] outputByteArray = Decrypt(inputByteArray);
            return outputByteArray == null ? string.Empty : ASCIIEncoding.ASCII.GetString(outputByteArray);
        }
        #endregion

        #region public byte[] Decrypt(byte[] srcData)
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="sourceData">需要解密的数据</param>
        /// <returns>解密后的数据</returns>
        public byte[] Decrypt(byte[] sourceData)
        {
            try
            {
                byte[] rgbKey = Encoding.ASCII.GetBytes(encryptKey);
                byte[] rgbIV = keys;
                byte[] inputByteArray = sourceData;
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion


    }
}
