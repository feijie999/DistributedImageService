using System;
using System.Threading.Tasks;
using ImageCore.Models;

namespace ImageCore.Persistence.EntityFramework
{
    public class EntityFrameworkFileStore : IFileStore
    {

        public EntityFrameworkFileStore()
        {

        }

        public Task<string> SaveAsync(bool isTemp = false)
        {
            throw new NotImplementedException();
        }

        public Task<string> PersistenceTempFile(string fileId)
        {
            throw new NotImplementedException();
        }

        public Task<ImageDataDto> GetImageData(Guid imageId)
        {
            throw new NotImplementedException();
        }
    }
}
