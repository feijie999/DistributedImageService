using ImageCore;
using ImageCore.Enums;
using SixLabors.ImageSharp;
using SixLabors.Primitives;

namespace ImageApi.Core
{
    public class ImageFileInfo : WebFileInfo
    {
        private Size _size;

        public Size Size
        {
            get
            {
                if (!_size.IsEmpty) return _size;
                using (var image = Image.Load(File.OpenReadStream()))
                {
                    _size = image.Size();
                }
                return _size;
            }
        }
    }
}