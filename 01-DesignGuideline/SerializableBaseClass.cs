/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�SerializableBaseClass.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ02��20��
 * * �ļ���ʶ��736FBD08-7EDA-47C1-B294-9E3B49305977
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace Codest
{
    /// <summary>
    /// Ϊ�����ṩXML���л������������л��������л�����
    /// ʵ����ICloneable�ӿ�
    /// </summary>
    /// <typeparam name="T">�̳��������</typeparam>
    [Serializable()]
    public abstract class SerializableBaseClass<T> : ICloneable
    {
        #region public virtual byte[] BinarySerialize()
        /// <summary>
        /// ������ж��������л�
        /// </summary>
        /// <returns>���л�����</returns>
        public virtual byte[] BinarySerialize()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, this);
            byte[] buffer = memoryStream.ToArray();
            memoryStream.Close();
            return buffer;
        }
        #endregion

        #region public virtual string XMLSerialize()
        /// <summary>
        /// �������XML���л�
        /// </summary>
        /// <returns>XML���л�����</returns>
        public virtual string XMLSerialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(GetType());
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, this);
            byte[] buffer = memoryStream.ToArray();
            string xml = Encoding.ASCII.GetString(buffer);
            memoryStream.Close();
            return xml;
        }
        #endregion

        #region  public static T DeSerialize(byte[] binary)
        /// <summary>
        /// ������ж����Ʒ����л�
        /// </summary>
        /// <param name="binary">���������л�����</param>
        /// <returns>�����л���Ķ�����ʧ���򷵻�null</returns>
        public static T Deserialize(byte[] binary)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(binary);
            T obj = (T)formatter.Deserialize(memoryStream);
            memoryStream.Close();
            return obj;
        }
        #endregion 

        #region public static T DeSerialize(string xmlString)
        /// <summary>
        /// �������XML�����л�
        /// </summary>
        /// <param name="xmlString">XML���л�����</param>
        /// <returns>�����л���Ķ�����ʧ���򷵻�null</returns>
        public static T Deserialize(string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            byte[] buffer = Encoding.ASCII.GetBytes(xmlString);
            MemoryStream memoryStream = new MemoryStream(buffer);
            T obj = (T)xmlSerializer.Deserialize(memoryStream);
            return obj;
        }
        #endregion

        #region public static bool TryDeSerialize(byte[] binary, ref T obj)
        /// <summary>
        /// ����ʹ�ö��������ݶ�����з����л�
        /// </summary>
        /// <param name="binary">���л�����������</param>
        /// <param name="obj">���ض�������</param>
        /// <returns>�����л��Ƿ�ɹ�</returns>
        public static bool TryDeserialize(byte[] binary, ref T obj)
        {
            try
            {
                obj = Deserialize(binary);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region public static bool TryDeSerialize(string  xmlString, ref T obj)
        /// <summary>
        /// ����ʹ��XML���ݶ�����з����л�
        /// </summary>
        /// <param name="xmlString">���л�XML����</param>
        /// <param name="obj">���ض�������</param>
        /// <returns>�����л��Ƿ�ɹ�</returns>
        public static bool TryDeserialize(string  xmlString, ref T obj)
        {
            try
            {
                obj = Deserialize(xmlString);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion 

        #region public virtual T Copy()
        /// <summary>
        /// ��ɶ����ǳ����
        /// </summary>
        /// <returns>����ĸ���</returns>
        public virtual T Copy()
        {
            return (T)Clone();
        }
        #endregion

        #region ICloneable ��Ա
        /// <summary>
        /// ��ɶ����ǳ����
        /// </summary>
        /// <returns>����ĸ���</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion


    }

}
