using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core;
using ImageCore;
using ImageCore.Enums;
using ImageCore.Extensions;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace ImageApi.Controllers
{
    /// <summary>
    /// 图片服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileStore _fileStore;
        private readonly IImageService _imageService;
        private readonly IFileProvider _fileProvider;
        private readonly ImageOption _imageOption;

        /// <inheritdoc />
        public ImageController(IFileStore fileStore, IImageService imageService, ImageOption imageOption, IFileProvider fileProvider)
        {
            _fileStore = fileStore;
            _imageService = imageService;
            _imageOption = imageOption;
            _fileProvider = fileProvider;
        }

        /// <summary>
        /// 图片链接
        /// </summary>
        /// <param name="parameterFixer"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("/img/{size}/t{imageType}t{yearMonth}-{id}.{format}")]
        [HttpGet]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index([FromServices]IImageParameterFixer parameterFixer, [FromRoute]ImageParameter parameter)
        {
            parameter = parameter.GetFixed(parameterFixer);
            var relativePath = parameter.GetRelativePath();
            var physicalPath = Path.GetFullPath("App_Data\\"+relativePath);
            IFileInfo fileInfo;
            var contentType = "image/" + parameter.Format;
            if (System.IO.File.Exists(physicalPath))
            {
                 fileInfo = _fileProvider.GetFileInfo(relativePath);
                return File(fileInfo.CreateReadStream(), contentType);
            }
            var physicalFolder = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(physicalFolder))
            {
                Directory.CreateDirectory(physicalFolder);
            }
            var imageData = await _fileStore.GetImageData(Guid.Parse(parameter.Id));
            _imageService.ImageCastAndSaveToFile(imageData.Bytes, parameter.Size, parameter.ImageFormat, physicalPath);
            fileInfo = _fileProvider.GetFileInfo(relativePath);
            return File(fileInfo.CreateReadStream(), contentType);
        }

        /// <summary>
        /// 返回的格式: "img/{size}/default-{name}.{format}", "img/{size}/t{imageType}t{yearMonth}-{id}.{format}"
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isTemp">是否是临时图片，临时图片将定期清理</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>
        /// 例如 /img/full/t20t201902-94E0437664E3FA99C094E0437664E3FA99C0.png
        /// 例如 /img/s80x80/t20t201902-94E0437664E3FA99C094E0437664E3FA99C0.png
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Upload([FromFile] ImageFileInfo file,bool isTemp = false, BusinessType businessType=BusinessType.Default)
        {
            file.Validate(_imageOption);
            var image = file.ToImage();
            var imageKey = await _fileStore.SaveAsync(image, isTemp);
            return Ok(imageKey);
        }
    }
}