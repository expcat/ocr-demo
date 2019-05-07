using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OCR.Test.Models
{
    public class UploadViewModel
    {
        [Required]
        [Display(Name = "证件正页")]
        public IFormFile FrontFile { get; set; }

        [Display(Name = "证件副页")]
        public IFormFile BackFile { get; set; }
    }
}
