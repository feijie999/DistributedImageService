using System;
using Microsoft.Extensions.DependencyInjection;

namespace ImageCore.Extensions
{
    public static class ImageServiceExtensions
    {
        public static ImageServiceBuilder AddImageService(this IServiceCollection service, Action<ImageOption> action = null)
        {
            service.AddTransient<IImageService, ImageService>();
            var imageOption = new ImageOption();
            action?.Invoke(imageOption);
            service.AddSingleton(imageOption);
            service.AddTransient(imageOption.FileStoreFactory);
            var serviceBuilder= new ImageServiceBuilder(service, imageOption);
            return serviceBuilder;
        }
    }
}