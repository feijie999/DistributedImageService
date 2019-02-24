using System;
using System.ComponentModel.DataAnnotations;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace ImageCore.Enums
{
    [Flags]
    public enum ImageFormat
    {
        Jpeg = 1,
        Png = 2,
        Gif = 4,
        Bmp = 8
    }

    public static class ImageFormatExtensions
    {
        public static ImageFormat GetFormatFromExtension(this string ext)
        {
            ext = ext.ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                default:
                    throw new ValidationException("错误的文件格式:"+ext);
            }
        }

        public static string GetExtension(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Png:
                    return ".png";
                case ImageFormat.Bmp:
                    return ".bmp";
                case ImageFormat.Gif:
                    return ".gif";
                case ImageFormat.Jpeg:
                default:
                    return ".jpg";
            }
        }

        public static IImageFormat GetFormat(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Png:
                    return PngFormat.Instance;
                case ImageFormat.Bmp:
                    return BmpFormat.Instance;
                case ImageFormat.Gif:
                    return GifFormat.Instance;
                case ImageFormat.Jpeg:
                default:
                    return JpegFormat.Instance;
            }
        }
    }
}