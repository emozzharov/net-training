/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：DSAEncrypt.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时01分16秒
 * * 文件标识：056C74BA-5776-4E24-9BBB-DE22E25481B7
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Codest.Encode;
using Codest.Net;

namespace Codest.Encode
{
    /// <summary>
    /// 实现对数据进行DSA签名和验证
    /// </summary>
    public class DSAEncrypt : SerializableBaseClass<DSAEncrypt>
    {
        #region 成员变量
        /// <summary>
        /// 提供DSA算法
        /// </summary>
        protected DSACryptoServiceProvider dsaCryptoServiceProvider;
        #endregion

        #region 接口封装

        #endregion

        #region 构造/析构函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DSAEncrypt()
        {
            dsaCryptoServiceProvider = new DSACryptoServiceProvider();
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~DSAEncrypt()
        {
            dsaCryptoServiceProvider.Clear();
        }
        #endregion

        #region public byte[] GetSignature(string srcData)
        /// <summary>
        /// 对字符串数据进行签名
        /// </summary>
        /// <param name="sourceData">字符串数据</param>
        /// <returns>DSA签名</returns>
        public byte[] GetSignature(string sourceData)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(sourceData);
            return GetSignature(binaryData);
        }
        #endregion

        #region public byte[] GetSignature(byte[] srcData)
        /// <summary>
        /// 对二进制数据进行签名
        /// </summary>
        /// <param name="sourceData">二进制数据</param>
        /// <returns>DSA签名</returns>
        public byte[] GetSignature(byte[] sourceData)
        {
            byte[] sign = dsaCryptoServiceProvider.SignData(sourceData);
            return sign;
        }
        #endregion

        #region public bool VerifySignature(byte[] srcData, byte[] signature)
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sourceData">需要验证的数据</param>
        /// <param name="signature">DSA签名</param>
        /// <returns>签名是否正确</returns>
        public bool VerifySignature(byte[] sourceData, byte[] signature)
        {
            bool ver = dsaCryptoServiceProvider.VerifyData(sourceData, signature);
            return ver;
        }
        #endregion

        #region public bool VerifySignature(string srcData, byte[] signature)
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sourceData">需要验证的数据</param>
        /// <param name="signature">DSA签名</param>
        /// <returns>签名是否正确</returns>
        public bool VerifySignature(string sourceData, byte[] signature)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(sourceData);
            bool isVerified = dsaCryptoServiceProvider.VerifyData(binaryData, signature);
            return isVerified;
        }
        #endregion
    }

}
