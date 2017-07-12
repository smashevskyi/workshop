using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class ImageViewModel
    {
        //public ImageViewModel() { }
        [Key]
        public int ImageId { get; set; }

        [Required]
        [MaxLength(32, ErrorMessage = "Максимальная длина 32 символов")]
        public string Name { get; set; }

        //[Required]
        //public int Position { get; set; } // = Id

        [Required]
        public bool Visible { get; set; } // false

    }
}