using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FileInfo = ImageCore.FileInfo;

namespace ImageApi.Core
{
    public class WebFileInfo : FileInfo
    {
        public override string[] Filters => new []{"dll","png","jpg"};
        protected override Task CopyToAsync(Stream stream)
        {
           return File.CopyToAsync(stream);
        }

        private IFormFile _file;
        public IFormFile File
        {
            get => _file;
            set
            {
                if (value != null)
                {
                    this._file = value;
                    this.FileType = this._file.ContentType;
                    this.Length = this._file.Length;
                    this.Extension = this._file.FileName.Substring(_file.FileName.LastIndexOf('.'));
                    if (string.IsNullOrEmpty(this.FileName))
                        this.FileName = this.FileName;
                }
            }
        }

        private byte[] _fileBytes;

        public byte[] FileBytes
        {
            get
            {
                if (_fileBytes != null || _file == null) return _fileBytes;
                var fileStream = _file.OpenReadStream();
                _fileBytes = new byte[_file.Length];
                _file.OpenReadStream().Read(_fileBytes, 0, _fileBytes.Length);
                fileStream.Flush();
                return _fileBytes;
            }
            set => _fileBytes = value;
        }
    }
}
