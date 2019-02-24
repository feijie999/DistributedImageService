using ImageCore.Enums;
using SixLabors.ImageSharp;

namespace ImageCore
{
    public interface IImageService
    {
        byte[] ImageCast(byte[] bytes, ImageSize imageSize,ImageFormat format);

        void ImageCastAndSaveToFile(byte[] bytes, ImageSize imageSize, ImageFormat format,string physicalPath);
    }
}