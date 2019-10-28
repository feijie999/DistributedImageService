using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageCore.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace ImageCore
{
    public class ImageService : IImageService
    {
        public byte[] ImageCast(byte[] bytes,ImageSize imageSize, ImageFormat format)
        {
            using var image = Image.Load(bytes);
            image.Mutate(x => x
                .Resize(imageSize.GetSize()));
            using var stream = new MemoryStream();
            var encoder =
                image.GetConfiguration().ImageFormatsManager.FindEncoder(format.GetFormat());
            image.Save(stream, encoder);
            stream.Seek(0, SeekOrigin.Begin);
            var result = new byte[stream.Length];
            stream.Read(result, 0, result.Length);
            return result;
        }

        public void ImageCastAndSaveToFile(byte[] bytes, ImageSize imageSize, ImageFormat format, string physicalPath)
        {
            using var image = Image.Load(bytes);
            var encoder =
                image.GetConfiguration().ImageFormatsManager.FindEncoder(format.GetFormat());
            image.Mutate(x => x
                .Resize(imageSize.GetSize()));
            image.Save(physicalPath, encoder);
        }

        public string GenerateKey(BusinessType type, Guid id, ImageFormat format)
        {
            var yearMonth = DateTime.Now.ToString("yyyyMM");
            return string.Format("t{0}t{3}-{1:N}{2}", type, id, format.GetExtension(), yearMonth);
        }
    }
}
