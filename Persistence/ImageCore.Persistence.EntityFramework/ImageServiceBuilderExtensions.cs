using System;
using ImageCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ImageCore.Persistence.EntityFramework
{
    public static class ImageServiceBuilderExtensions
    {
        public static ImageServiceBuilder UseEntityFrameworkStore(this ImageServiceBuilder serviceBuilder,
            Action<DbContextOptionsBuilder> action)
        {
            serviceBuilder.Service.AddDbContext<ImageDbContext>(action);
            serviceBuilder.ImageOption.FileStoreFactory = provider =>
            {
                var dbContext = provider.GetService<ImageDbContext>();
                var imageOption = provider.GetService<ImageOption>();
                var imageService = provider.GetService<IImageService>();
                return new EntityFrameworkFileStore(dbContext, imageOption, imageService);
            };

            serviceBuilder.Service.AddTransient(serviceBuilder.ImageOption.FileStoreFactory);
            return serviceBuilder;
        }
    }
}