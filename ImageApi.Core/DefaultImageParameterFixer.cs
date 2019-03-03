using System;
using ImageCore.Enums;

namespace ImageApi.Core
{
    public class DefaultImageParameterFixer : IImageParameterFixer
    {
        public ImageParameter Fix(ImageParameter parameter)
        {
            parameter.Year = parameter.YearMonth / 100;
            parameter.Month = parameter.YearMonth % 100;
            if (parameter.Year < 2017 || parameter.Year > DateTime.Now.Year + 1 || parameter.Month < 1 ||
                parameter.Month > 12)
            {
                throw new Exception("错误的年月格式");
            }
            parameter.Format = parameter.Format.Replace("jpg", "jpeg");
            parameter.ImageFormat = parameter.Format.GetFormatFromExtension();
            return parameter;
        }
    }
}