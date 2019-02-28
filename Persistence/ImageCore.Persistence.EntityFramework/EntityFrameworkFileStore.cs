using System;
using System.Linq;
using System.Threading.Tasks;
using ImageCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageCore.Persistence.EntityFramework
{
    public class EntityFrameworkFileStore : IFileStore
    {
        private readonly ImageDbContext _imageDbContext;
        private readonly ImageOption _imageOption;
        private readonly IImageService _imageService;

        public EntityFrameworkFileStore(ImageDbContext imageDbContext, IOptions<ImageOption> imageOption, IImageService imageService)
        {
            _imageDbContext = imageDbContext;
            _imageService = imageService;
            _imageOption = imageOption.Value;
        }

        public async Task<string> SaveAsync(ImageDto imageDto, bool isTemp = false)
        {
            var image = new Image()
            {
                Height = imageDto.Height,
                Width = imageDto.Width,
                Length = imageDto.Length,
                BusinessType = imageDto.BusinessType,
                Bytes = imageDto.Bytes,
                CreateTime = imageDto.CreateTime,
                Format = imageDto.Format,
                Id = Guid.NewGuid(),
                IsTemp = isTemp,
                Name = imageDto.Name
            };
            var key = _imageService.GenerateKey(image.BusinessType, image.Id, image.Format);
            image.Key = key;
            await _imageDbContext.Images.AddAsync(image);
            return key;
        }

        public async Task PersistenceTempFile(string imageKey)
        {
            var image = await _imageDbContext.Images.FirstAsync(x => x.Key == imageKey);
            image.IsTemp = false;
            await _imageDbContext.SaveChangesAsync();
        }

        public Task<ImageDataDto> GetImageData(Guid imageId)
        {
            return _imageDbContext.Images.Where(x => x.Id == imageId).Select(x => new ImageDataDto()
            {
                Format = x.Format,
                Bytes = x.Bytes
            }).FirstOrDefaultAsync();
        }
    }
}
