/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：DataManager.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分43秒
 * * 文件标识：124F76B9-05EE-4B2A-9400-A233BC1D4BF3
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// 所有数据库管理器的基类.
    /// </summary>
    public abstract class DataManager : BaseClass
    {
        private int execNum = 0;

        /// <summary>
        /// 保存当前管理器中所有已分配的更新器.
        /// </summary>
        private Hashtable dataUpdaterColl;
        private string connString = string.Empty;
        private string dbscr = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DataManager()
        {
            this.dataUpdaterColl = new Hashtable();
        }

        /// <summary>
        /// 获取或设置数据库连接字符串.
        /// </summary>
        public string ConnString
        {
          get { return this.connString; }
          set { this.connString = value; }
        }

        /// <summary>
        /// 获取当前数据库连接查询的次数.
        /// </summary>
        public int ExecNum
        {
            get { return this.execNum; }
            set { this.execNum = value; }
        }

        /// <summary>
        /// 获取或指定数据库源.
        /// </summary>
        public string DataSource
        {
            get { return this.dbscr; }
            set { this.dbscr = value; }
        }

        /// <summary>
        /// Gets dataupdatercoll.
        /// </summary>
        private protected Hashtable DataUpdaterColl => this.dataUpdaterColl;

        /// <summary>
        /// 打开数据库.
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// 使用数据库连接字符串打开数据库.
        /// </summary>
        public abstract void OpenByConnString();

        /// <summary>
        /// 关闭SQL连接.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 执行SQL语句.
        /// </summary>
        /// <param name="sQLCmd">SQL语句.</param>
        /// <returns>受响应的行数.</returns>
        public abstract int Exec(string sQLCmd);

        /// <summary>
        /// 执行SQL语句，将响应的数据填充到DataTable中，不能进行更新操作.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>some return.</returns>
        public abstract DataTable SelectDataTable(string sQLCmd);

        /// <summary>
        /// 选择一定范围记录的Select语句.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <param name="srcTalbe">srcTable param.</param>
        /// <param name="startRecord">startRecord param.</param>
        /// <param name="maxRecord">max record param.</param>
        /// <returns>some return.</returns>
        public abstract DataTable SelectDataTable(string sQLCmd, string srcTalbe, int startRecord, int maxRecord);

        /// <summary>
        /// 实现分页的Select.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <param name="srcTalbe">srcTable param.</param>
        /// <param name="pageSize">startRecord param.</param>
        /// <param name="pageID">max record param.</param>
        /// <returns>some return.</returns>
        public abstract DataTable SelectPage(string sQLCmd, string srcTalbe, int pageSize, int pageID);

        /// <summary>
        /// 执行删除操作的SQL语句.
        /// </summary>
        /// <param name="sQLCmd">some param.</param>
        /// <returns>some return.</returns>
        public abstract bool Delete(string sQLCmd);

        /// <summary>
        /// 分配一个更新器.
        /// </summary>
        /// <returns>some return.</returns>
        public abstract DataUpdater AllocateDataUpdater();

        /// <summary>
        /// 释放当前实例中所有的更新器.
        /// </summary>
        public virtual void ReleaseAllDataUpdater()
        {
            foreach (DictionaryEntry de in this.dataUpdaterColl)
            {
                DataUpdater updater = (DataUpdater)de.Value;
                updater.Dispose();
            }

            this.dataUpdaterColl.Clear();
        }

        /// <summary>
        /// 释放一个更新器.
        /// </summary>
        /// <param name="updater">更新器.</param>
        public virtual void ReleaseDataUpdater(DataUpdater updater)
        {
            this.dataUpdaterColl.Remove(updater?.UpdaterID);
            updater.Dispose();
        }

        /// <summary>
        /// 释放由当前对象控制的所有资源.
        /// </summary>
        /// <param name="disposing">显式调用.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // 释放托管资源
            }

            // 释放非托管资源
            base.Dispose(disposing);
        }
    }
}
