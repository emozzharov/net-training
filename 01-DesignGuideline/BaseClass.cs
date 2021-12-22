/*******************************************************************************
 * * ��Ȩ����(C) CODEST.ORG. �������ѭGPLЭ�顣
 * * �ļ����ƣ�BaseClass.cs
 * * �������ߣ�ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * �������ڣ�2009��08��24�� 18ʱ02��17��
 * * �ļ���ʶ��F78FB93F-3459-4CF8-82CB-4BEF90B3CEAA
 * * ����ժҪ��
 * *******************************************************************************/

using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Codest
{
    /// <summary>
    /// �޲������޷���ֵ���й�����.
    /// </summary>
    public delegate void NullParamEvent();

    /// <summary>
    /// Icyplayer������Ļ���
    /// ʵ��IDisposable, ICloneable�ӿ�.
    /// </summary>
    public abstract class BaseClass : IDisposable, ICloneable
    {
        /// <summary>
        /// �й���Դ����.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// ָʾ�����Ƿ��Ѿ�����.
        /// </summary>
        protected internal bool disposed = false;

        /// <summary>
        /// BaseClass���캯��
        /// </summary>
        protected BaseClass()
        {
        }

        /// <summary>
        /// BaseClass��������
        /// </summary>
        ~BaseClass()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// ��ɶ����ǳ����.
        /// </summary>
        /// <returns>����ĸ���.</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// ��ʽ��������.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="disposing">false��ϵͳ���ã�true���ֶ�����.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // �ͷ��й���Դ
                if (this.components != null)
                {
                    this.components.Dispose();
                }

                this.disposed = true;
            }

            // �ͷŷ��й���Դ
            // base.Dispose(disposing);
        }
    }
}
