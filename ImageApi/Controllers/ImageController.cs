using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload([FromFileAttribute] WebFileInfo fileInfo)
        {
            await fileInfo.SaveAs();
            return Ok();
        }
    }
}