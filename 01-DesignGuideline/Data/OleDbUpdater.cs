/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：OleDbUpdater.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分52秒
 * * 文件标识：61502EEA-229B-4503-9A25-0DD25ED0B4B3
 * * 内容摘要：
 * *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace Codest.Data
{
    /// <summary>
    /// OleDb数据库更新器.
    /// </summary>
    public class OleDbUpdater : DataUpdater
    {
        /// <summary>
        /// 当前更新器所使用的数据库管理器.
        /// </summary>
        private OleDbManager dataManager;

        /// <summary>
        /// 当有更新操作时，用来保存数据信息.
        /// </summary>
        private System.Data.OleDb.OleDbDataAdapter dap;

        /// <summary>
        /// 当有更新操作时，配合OleDbDataAdapter使用.
        /// </summary>
        private System.Data.OleDb.OleDbCommandBuilder cmdb;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">容器中唯一的更新器ID.</param>
        /// <param name="manager">数据库管理器.</param>
        public OleDbUpdater(int id, OleDbManager manager)
            : base(id)
        {
            this.dataManager = manager;
        }

        /// <summary>
        /// 使对象将进入修改模式状态
        /// 用户可以修改返回结果，包括删除、修改和增加行
        /// 再调用Update(DataTable)进行更新操作
        /// 此后，对象将退出修改模式状态.
        /// </summary>
        /// <param name="sQLCmd">SQL语句.</param>
        /// <returns>查询响应结果.</returns>
        public override DataTable SelectWithUpdate(string sQLCmd)
        {
            this.dataManager.ExecNum++;
            System.Data.DataTable dt = new DataTable();
            this.dap = new OleDbDataAdapter(sQLCmd, this.dataManager.Connection);
            this.cmdb = new OleDbCommandBuilder(this.dap);
            this.dap.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 使对象进入修改模式
        /// 用户可以在返回表结构的DataTable中添加数据
        /// 再调用Update(DataTable)进行更新操作
        /// 此后，对象将退出修改模式状态.
        /// </summary>
        /// <param name="tableName">需要插入的表名称.</param>
        /// <returns>要插入目标表的结构.</returns>
        public override DataTable InsertMode(string tableName)
        {
            this.dataManager.ExecNum++;
            System.Data.DataTable dt = new DataTable();
            this.dap = new OleDbDataAdapter("select * from [" + tableName + "] where false", this.dataManager.Connection);
            this.cmdb = new OleDbCommandBuilder(this.dap);
            this.dap.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 关闭修改模式,并根据DataTable进行更新操作.
        /// </summary>
        /// <param name="dataTableSource">要提交的数据表.</param>
        public override void Update(System.Data.DataTable dataTableSource)
        {
            this.dataManager.ExecNum++;
            this.dap.Update(dataTableSource);
            this.DecideRelease();
        }

        /// <summary>
        /// 释放当前更新器.
        /// </summary>
        public override void Release()
        {
            this.dataManager.ReleaseDataUpdater(this);
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
                this.dap.Dispose();
                // 释放托管资源
            }

            // 释放非托管资源
            this.dataManager = null;
            this.cmdb = null;
            this.dap = null;
            this.dap.Dispose();
            base.Dispose(disposing);
        }
    }
}
