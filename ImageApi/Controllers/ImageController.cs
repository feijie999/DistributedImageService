using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core;
using ImageCore;
using ImageCore.Enums;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;

namespace ImageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileStore _fileStore;
        private readonly IImageService _imageService;
        private readonly ImageOption _imageOption;

        public ImageController(IFileStore fileStore, IImageService imageService, IOptions<ImageOption> imageOption)
        {
            _fileStore = fileStore;
            _imageService = imageService;
            _imageOption = imageOption.Value;
        }

        [Route("img/{size}/t{imageType}t{yearMonth}-{id}.{format}")]
        public async Task<IActionResult> Index(ImageParameter parameter)
        {
            var physicalPath = Path.GetFullPath(parameter.GetRelativePath());
            var contentType = "image/" + parameter.Format;
            if (System.IO.File.Exists(physicalPath))
            {
                return File(physicalPath, contentType);
            }
            var physicalFolder = Path.GetDirectoryName(physicalPath);
            if (!Directory.Exists(physicalFolder))
            {
                Directory.CreateDirectory(physicalFolder);
            }
            var imageData = await _fileStore.GetImageData(Guid.Parse(parameter.Id));
            _imageService.ImageCastAndSaveToFile(imageData.Bytes, parameter.Size, parameter.ImageFormat, physicalPath);
            return File(physicalPath, contentType);
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
            var image = file.ToImage();
            var imageKey = await _fileStore.SaveAsync(image, isTemp);
            return Ok(imageKey);
        }
    }
}