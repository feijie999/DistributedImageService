using System;
using System.Threading.Tasks;
using ImageCore.Models;

namespace ImageCore
{
    /// <summary>
    /// 文件存储
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="isTemp">是否是临时文件,如果是零时文件系统将会定期清理</param>
        /// <returns>返回fileId</returns>
        Task<string> SaveAsync(bool isTemp = false);

        /// <summary>
        /// 持久化零时文件
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <returns>返回文件token</returns>
        Task<string> PersistenceTempFile(string fileId);

        Task<ImageDataDto> GetImageData(Guid imageId);
    }
}