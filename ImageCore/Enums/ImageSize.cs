using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.Primitives;

namespace ImageCore.Enums
{
    public enum ImageSize
    {
        Full = 0,
        S80X80 = 1,
        S150X50 = 2,
        S100X100 = 3,
        S160X160 = 4,
        S190X190 = 6,
        S250X250 = 7,
        S300X300 = 8,
        S450X450 = 9,
        S600X600 = 10,
        S1920X1920 = 11,
        H100 = 5
    }

    public static class ImageSizeExtensions
    {
        public static Size GetSize(this ImageSize size)
        {
            switch (size)
            {
                case ImageSize.S80X80:
                    return new Size(80, 80);
                case ImageSize.S100X100:
                    return new Size(100, 100);
                case ImageSize.S150X50:
                    return new Size(150, 50);
                case ImageSize.S160X160:
                    return new Size(160, 160);
                case ImageSize.S190X190:
                    return new Size(190, 190);
                case ImageSize.S250X250:
                    return new Size(250, 250);
                case ImageSize.S300X300:
                    return new Size(300, 300);
                case ImageSize.S450X450:
                    return new Size(450, 450);
                case ImageSize.S600X600:
                    return new Size(600, 600);
                case ImageSize.H100:
                    return new Size(0, 100);
                default:
                case ImageSize.Full:
                    return new Size();
            }
        }
    }
}
