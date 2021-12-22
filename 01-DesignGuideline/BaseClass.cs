/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：BaseClass.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时02分17秒
 * * 文件标识：F78FB93F-3459-4CF8-82CB-4BEF90B3CEAA
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Codest
{
    /// <summary>
    /// 无参数且无返回值的托管类型.
    /// </summary>
    public delegate void NullParamEvent();

    /// <summary>
    /// Icyplayer所有类的基类
    /// 实现IDisposable, ICloneable接口.
    /// </summary>
    public abstract class BaseClass : IDisposable, ICloneable
    {
        /// <summary>
        /// 托管资源容器.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// 指示对象是否已经析构.
        /// </summary>
        protected internal bool disposed = false;

        /// <summary>
        /// BaseClass构造函数
        /// </summary>
        protected BaseClass()
        {
        }

        /// <summary>
        /// BaseClass析构函数
        /// </summary>
        ~BaseClass()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 完成对象的浅复制.
        /// </summary>
        /// <returns>对象的副本.</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 显式撤销对象.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 撤销对象.
        /// </summary>
        /// <param name="disposing">false：系统调用，true：手动调用.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // 释放托管资源
                if (this.components != null)
                {
                    this.components.Dispose();
                }

                this.disposed = true;
            }

            // 释放非托管资源
            // base.Dispose(disposing);
        }
    }
}
