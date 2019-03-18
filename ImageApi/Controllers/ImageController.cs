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
        /// url格式：/img/{size}/t{imageType}t{yearMonth}-{id}.{format}
        /// 例如：http://172.16.4.64:8089/img/s250x250/tDefaultt201903-7a35a53d36334e008f282a9927043df5.jpg
        /// {size},枚举为:
        /// Full = 0, S80X80 = 1,S150X50 = 2, S100X100 = 3,S160X160 = 4,S190X190 = 6,S250X250 = 7,S300X300 = 8,S450X450 = 9,S600X600 = 10,S1920X1920 = 11,H100 = 5
        /// t{imageType}t{yearMonth}-{id}.{format}为<see cref="Upload"/> 接口返回的ImageKey
        /// </summary>
        /// <param name="parameterFixer"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("/img/{size}/t{imageType}t{yearMonth}-{id}.{format}")]
        [HttpGet]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index([FromServices] IImageParameterFixer parameterFixer,
            [FromRoute] ImageParameter parameter)
        {
            parameter = parameter.GetFixed(parameterFixer);
            var relativePath = parameter.GetRelativePath();
            var physicalPath = Path.GetFullPath(relativePath);
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
            if (imageData == null)
            {
                return NotFound();
            }
            _imageService.ImageCastAndSaveToFile(imageData.Bytes, parameter.Size, parameter.ImageFormat, physicalPath);
            fileInfo = _fileProvider.GetFileInfo(relativePath);
            return File(fileInfo.CreateReadStream(), contentType);
        }

        /// <summary>
        /// 返回的格式: "{size}/default-{name}.{format}"
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isTemp">是否是临时图片，临时图片将定期清理</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>
        /// 例如 /img/full/t20t201902-94E0437664E3FA99C094E0437664E3FA99C0.png
        /// 例如 /img/s80x80/t20t201902-94E0437664E3FA99C094E0437664E3FA99C0.png
        /// </returns>
        [HttpPost]
        [HttpHead("")]
        public async Task<IActionResult> Upload([FromFile] ImageFileInfo file, bool isTemp = false,
            BusinessType businessType = BusinessType.Default)
        {
            file.Validate(_imageOption);
            var image = file.ToImage();
            var imageKey = await _fileStore.SaveAsync(image, isTemp);
            return Ok(imageKey);
        }
    }
}