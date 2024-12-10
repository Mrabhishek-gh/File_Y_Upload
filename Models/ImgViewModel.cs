using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagePreviewUpload.Models
{
    public class ImgViewModel
    {
        [Required]
        [Display(Name = "Upload File")]
        public HttpPostedFileBase FileAttach { get; set; }
        public string UserName { get; set; }
        public List<ImgObj> ImgLst { get; set; }

    }
}