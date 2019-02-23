using System;

namespace ImageCore.Enums
{
    /// <summary>
    /// 存储模式
    /// </summary>
    [Flags]
    public enum StorageMode
    {
        /// <summary>
        /// 本地文件存储
        /// </summary>
        Local = 1,

        /// <summary>
        /// 数据库存储
        /// </summary>
        DataBase = 2,

        /// <summary>
        /// 分布式存储
        /// </summary>
        Distributed = Local | DataBase
    }
}