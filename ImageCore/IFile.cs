using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageCore
{
    public interface IFile
    {
        long Length { get; }

        string Name { get; }

        string FileName { get; }

        string FileType { get; }
        
        Stream OpenReadStream();

        void CopyTo(Stream target);
        
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken));
    }
}