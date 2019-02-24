using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace ImageCore
{
    public static class ImageProcessingContextExtensions
    {
        public static IImageProcessingContext<TPixel> Resize<TPixel>(this IImageProcessingContext<TPixel> source, Size target) where TPixel : struct, IPixel<TPixel>
        {
            if (target.Width == 0 && target.Height == 0)
            {
                return source;
            }

            var origin = source.GetCurrentSize();
            if (target.Width == 0)
            {
                var rate = (float)origin.Height / target.Height;
                if (rate <= 1)
                {
                    return source;
                }
                return source.Resize<TPixel>((int)(origin.Width / rate), target.Height, KnownResamplers.Bicubic, false);
            }

            if (target.Height == 0)
            {
                var rate = (float)origin.Width / target.Width;
                if (rate <= 1)
                {
                    return source;
                }
                return source.Resize<TPixel>(target.Width, (int)(origin.Height / rate), KnownResamplers.Bicubic, false);
            }

            var size = new Size();
            var r = Math.Max(((float)origin.Width / target.Width), ((float)origin.Height) / target.Height);
            if (r <= 1)
            {
                return source;
            }
            size.Width = (int)Math.Round(origin.Width / r);
            size.Height = (int)Math.Round(origin.Height / r);
            if (size.Width == 0)
            {
                size.Width = 1;
            }
            if (size.Height == 0)
            {
                size.Height = 1;
            }
            return source.Resize<TPixel>(size, KnownResamplers.Bicubic, false);
        }
    }
}