using System;
using System.ComponentModel.DataAnnotations;
using ImageCore.Enums;

namespace ImageCore.Models
{
    public class ImageDto
    {
        public DateTime CreateTime { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        public ImageFormat Format { get; set; }

        public BusinessType BusinessType { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public long Length { get; set; }

        public bool IsTemp { get; set; } = false;

        public byte[] Bytes { get; set; }
    }
}