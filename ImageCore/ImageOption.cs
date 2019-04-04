using System;
using ImageCore.Enums;

namespace ImageCore
{
    public class ImageOption
    {
        /// <summary>
        /// 文件格式过滤
        /// </summary>
        public string[] Filters { get; set; } = { ".jpg", ".png", ".bmp" };

        /// <summary>
        /// 可接收文件最大的大小单位kb，默认为2MB
        /// </summary>
        public long MaxContentLength { get; set; } = 1024 * 1024 * 2;

        /// <summary>
        /// 文件存储模式，默认为分布式存储
        /// </summary>
        public StorageMode StorageMode { get; set; } = StorageMode.Distributed;

        public Func<IServiceProvider, IFileStore> FileStoreFactory { get; set; } = provider => null;
    }
}