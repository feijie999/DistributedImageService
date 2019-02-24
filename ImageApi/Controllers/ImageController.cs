using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core;
using ImageCore;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ImageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileStore _fileStore;
        private readonly IImageService _imageService;

        public ImageController(IFileStore fileStore, IImageService imageService)
        {
            _fileStore = fileStore;
            _imageService = imageService;
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

        [HttpPost]
        public async Task<IActionResult> Upload([FromFileAttribute] WebFileInfo fileInfo)
        {

            await fileInfo.SaveAs();
            return Ok();
        }
    }
}