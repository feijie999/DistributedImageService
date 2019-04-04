using ImageCore.Enums;

namespace ImageCore.Models
{
    public class ImageDataDto
    {
        public byte[] Bytes { get; set; }

        public ImageFormat Format { get; set; }
    }
}
