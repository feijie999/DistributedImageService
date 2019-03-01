using System;
using System.Linq;

namespace ImageCore.Extensions
{
    public static class FileInfoExtensions
    {
        public static void Validate(this FileInfo fileInfo, ImageOption imageOption)
        {
            if (!imageOption.Filters.Contains(fileInfo.Extension.ToLower()))
            {
                throw new Exception("文件格式错误");
            }
            if (imageOption.MaxContentLength < fileInfo.Length)
            {
                throw new Exception($"文件必须小于{imageOption.MaxContentLength}");
            }
        }
    }
}