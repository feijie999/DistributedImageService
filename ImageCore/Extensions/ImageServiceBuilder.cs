using System;
using Microsoft.Extensions.DependencyInjection;

namespace ImageCore.Extensions
{
    public class ImageServiceBuilder
    {
        public IServiceCollection Service { get; set; }
        public ImageOption ImageOption { get; set; }

        public ImageServiceBuilder(IServiceCollection service,ImageOption imageOption)
        {
            Service = service;
            ImageOption = imageOption;
        }
    }
}