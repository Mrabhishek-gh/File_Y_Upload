using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagePreviewUpload.Models
{
    public class ImgObj
    {
        public int FileId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}