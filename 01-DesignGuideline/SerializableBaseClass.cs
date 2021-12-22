/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�SerializableBaseClass.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ02��20��
 * * �ļ���ʶ��736FBD08-7EDA-47C1-B294-9E3B49305977
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Codest
{
    /// <summary>
    /// Ϊ�����ṩXML���л������������л��������л�����
    /// ʵ����ICloneable�ӿ�.
    /// </summary>
    /// <typeparam name="T">�̳��������.</typeparam>
    [Serializable]
    public abstract class SerializableBaseClass<T> : ICloneable
    {
        /// <summary>
        /// ������ж��������л�.
        /// </summary>
        /// <returns>���л�����.</returns>
        public virtual byte[] BinarySerialize()
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            ser.Serialize(mStream, this);
            byte[] buf = mStream.ToArray();
            mStream.Close();
            return buf;
        }

        /// <summary>
        /// �������XML���л�.
        /// </summary>
        /// <returns>XML���л�����.</returns>
        public virtual string XMLSerialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            MemoryStream stream = new MemoryStream();
            xmlSerializer.Serialize(stream, this);
            byte[] buf = stream.ToArray();
            string xml = Encoding.ASCII.GetString(buf);
            stream.Close();
            return xml;
        }

        /// <summary>
        /// ������ж����Ʒ����л�.
        /// </summary>
        /// <param name="binary">���������л�����.</param>
        /// <returns>�����л���Ķ�����ʧ���򷵻�null.</returns>
        public T DeSerialize(byte[] binary)
        {
            BinaryFormatter ser = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream(binary);
            T o = (T)ser.Deserialize(mStream);
            mStream.Close();
            return o;
        }

        /// <summary>
        /// �������XML�����л�.
        /// </summary>
        /// <param name="xmlString">XML���л�����.</param>
        /// <returns>�����л���Ķ�����ʧ���򷵻�null.</returns>
        public T DeSerialize(string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            byte[] buf = Encoding.ASCII.GetBytes(xmlString);
            MemoryStream stream = new MemoryStream(buf);
            T o = (T)xmlSerializer.Deserialize(stream);
            return o;
        }

        /// <summary>
        /// ����ʹ�ö��������ݶ�����з����л�.
        /// </summary>
        /// <param name="binary">���л�����������.</param>
        /// <param name="obj">���ض�������.</param>
        /// <returns>�����л��Ƿ�ɹ�.</returns>
        public bool TryDeSerialize(byte[] binary, T obj)
        {
            try
            {
                obj = this.DeSerialize(binary);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ����ʹ��XML���ݶ�����з����л�.
        /// </summary>
        /// <param name="xmlString">���л�XML����.</param>
        /// <param name="obj">���ض�������.</param>
        /// <returns>�����л��Ƿ�ɹ�.</returns>
        public bool TryDeSerialize(string xmlString, T obj)
        {
            try
            {
                obj = this.DeSerialize(xmlString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��ɶ����ǳ����.
        /// </summary>
        /// <returns>����ĸ���.</returns>
        public virtual T Copy()
        {
            return (T)this.Clone();
        }

        /// <summary>
        /// ��ɶ����ǳ����.
        /// </summary>
        /// <returns>����ĸ���.</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
