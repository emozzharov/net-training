/*******************************************************************************
 * * 版权所有(C) CODEST.ORG. 本软件遵循GPL协议。
 * * 文件名称：SQLUpdater.cs
 * * 作　　者：ZhaoYu(email@zhaoyu.me) <http://www.zhaoyu.me/>
 * * 创建日期：2009年08月24日 18时00分58秒
 * * 文件标识：9FA64F08-E37B-4579-A18B-DEA743CFED03
 * * 内容摘要：
 * *******************************************************************************/

using System.Data;
using System.Data.SqlClient;

namespace Codest.Data
{
    /// <summary>
    /// SQL Server操作数据库更新器.
    /// </summary>
    public class SQLUpdater : DataUpdater
    {
        /// <summary>
        /// 当前更新器所使用的数据库管理器.
        /// </summary>
        private SQLManager dataManager;

        /// <summary>
        /// 当有更新操作时，用来保存数据信息.
        /// </summary>
        private SqlDataAdapter dap;

        /// <summary>
        /// 当有更新操作时，配合DataAdapter使用.
        /// </summary>
        private SqlCommandBuilder cmdb;

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="id">容器中唯一的ID.</param>
        /// <param name="manager">数据库管理器.</param>
        public SQLUpdater(int id, SQLManager manager)
            : base(id)
        {
            this.dataManager = manager;
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
                this.cmdb.Dispose();
                // 释放托管资源
            }

            // 释放非托管资源
            this.dataManager = null;
            this.cmdb = null;
            this.dap = null;
            base.Dispose(disposing);
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
            this.dap = new SqlDataAdapter(sQLCmd, this.dataManager.Conn);
            this.cmdb = new SqlCommandBuilder(this.dap);
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
            this.dap = new SqlDataAdapter("select * from [" + tableName + "] where 1=0", this.dataManager.Conn);
            this.cmdb = new SqlCommandBuilder(this.dap);
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
        /// 释放当前更新器。
        /// 如果AutoRelease=true，则调用Update()后会自动调用该方法。.
        /// </summary>
        public override void Release()
        {
            this.dataManager.ReleaseDataUpdater(this);
        }
    }
}
