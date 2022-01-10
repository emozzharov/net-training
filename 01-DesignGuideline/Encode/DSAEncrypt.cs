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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Codest.Encode;
using Codest.Net;

namespace Codest.Encode
{
    /// <summary>
    /// 实现对数据进行DSA签名和验证.
    /// </summary>
    public class DSAEncrypt : SerializableBaseClass<DSAEncrypt>
    {
        /// <summary>
        /// 提供DSA算法.
        /// </summary>
        private DSACryptoServiceProvider dsac;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public DSAEncrypt()
        {
            this.dsac = new DSACryptoServiceProvider();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~DSAEncrypt()
        {
            this.dsac.Clear();
        }

        /// <summary>
        /// 对字符串数据进行签名.
        /// </summary>
        /// <param name="srcData">字符串数据.</param>
        /// <returns>DSA签名.</returns>
        public byte[] GetSignature(string srcData)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(srcData);
            return this.GetSignature(binaryData);
        }

        /// <summary>
        /// 对二进制数据进行签名.
        /// </summary>
        /// <param name="srcData">二进制数据.</param>
        /// <returns>DSA签名.</returns>
        public byte[] GetSignature(byte[] srcData)
        {
            byte[] sign = this.dsac.SignData(srcData);
            return sign;
        }

        /// <summary>
        /// 验证签名.
        /// </summary>
        /// <param name="srcData">需要验证的数据.</param>
        /// <param name="signature">DSA签名.</param>
        /// <returns>签名是否正确.</returns>
        public bool VerifySignature(byte[] srcData, byte[] signature)
        {
            bool ver = this.dsac.VerifyData(srcData, signature);
            return ver;
        }

        /// <summary>
        /// 验证签名.
        /// </summary>
        /// <param name="srcData">需要验证的数据.</param>
        /// <param name="signature">DSA签名.</param>
        /// <returns>签名是否正确.</returns>
        public bool VerifySignature(string srcData, byte[] signature)
        {
            byte[] binaryData;
            binaryData = ASCIIEncoding.ASCII.GetBytes(srcData);
            bool ver = this.dsac.VerifyData(binaryData, signature);
            return ver;
        }
    }
}
