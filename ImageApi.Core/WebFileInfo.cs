using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ImageCore;
using Microsoft.AspNetCore.Http;
using FileInfo = ImageCore.FileInfo;

namespace ImageApi.Core
{
    public class WebFileInfo : FileInfo
    {
        protected override Task CopyToAsync(Stream stream)
        {
           return File.CopyToAsync(stream);
        }

        public IFormFile File { get; set; }

        public void Fix()
        {
            this.FileType = this.File.ContentType;
            this.Length = this.File.Length;
            this.Extension = this.File.FileName.Substring(File.FileName.LastIndexOf('.'));
            if (string.IsNullOrEmpty(this.FileName))
                this.FileName = this.File.FileName;
        }

        private byte[] _fileBytes;

        [JsonIgnore]
        public byte[] FileBytes
        {
            get
            {
                if (_fileBytes != null || File == null) return _fileBytes;
                using var fileStream = File.OpenReadStream();
                _fileBytes = new byte[File.Length];
                fileStream.Read(_fileBytes, 0, _fileBytes.Length);
                return _fileBytes;
            }
            set => _fileBytes = value;
        }
    }
}
