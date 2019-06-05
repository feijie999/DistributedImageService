using System;
using ImageCore.Enums;
using ImageCore.Models;
using ImageCore.Persistence.EntityFramework;

namespace ImageApi.Core
{
    public static class ImageFileInfoExtensions
    {
        public static ImageDto ToImage(this ImageFileInfo fileInfo)
        {
            ImageFormat imageFormat;
            switch (fileInfo.Extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                default:
                    throw new Exception("此文件不是图片类型");
            }

            var image = new ImageDto()
            {
                Format = imageFormat,
                Length = fileInfo.Length,
                Bytes = fileInfo.FileBytes,
                CreateTime = DateTime.Now,
                Name = fileInfo.FileName,
                BusinessType = BusinessType.Default,
                Width = fileInfo.Size.Width,
                Height = fileInfo.Size.Height
            };
            return image;
        }
    }
}