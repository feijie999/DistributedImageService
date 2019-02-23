using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCore.Enums
{
    /// <summary>
    /// 图片所关联的业务类型
    /// </summary>
    public enum BusinessType
    {
        /// <summary>
        /// 默认不指定业务
        /// </summary>
        Default = 0,

        /// <summary>
        /// 用户头像
        /// </summary>
        UserHead = 10,

        /// <summary>
        /// 产品图
        /// </summary>
        Product = 20,

        /// <summary>
        /// 内容图
        /// </summary>
        Content = 30
    }
}
