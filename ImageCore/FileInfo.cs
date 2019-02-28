using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCore
{
    public abstract class FileInfo
    {

        public string FileName { get; set; }
        public long Length { get; set; }
        public string Extension { get; set; }
        public string FileType { get; set; }
        

        public abstract string[] Filters { get; }

        public bool IsValid => !string.IsNullOrEmpty(this.Extension) && Filters.Contains(this.Extension);

        public virtual async Task<string> SaveAs(string destinationDir = null)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("没有需要保存的文件");

            if (destinationDir != null)
                Directory.CreateDirectory(destinationDir);

            var newName = DateTime.Now.Ticks;
            var newFile = Path.Combine(destinationDir ?? "", $"{newName}{this.Extension}");
            using (var fs = new FileStream(newFile, FileMode.CreateNew))
            {
                await CopyToAsync(fs);
                fs.Flush();
            }
            return newFile;
        }

        protected abstract Task CopyToAsync(Stream stream);
    }
}
