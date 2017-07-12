using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWorkshop.Models
{
    public class Image
    {
        public int ImageId { get; set; }

        public string Name { get; set; } // First photo

        // public string Description { get; set; } // My first photo
        public int Position { get; set; } // = Id

        [Display(Name = "Added on")]
        public DateTime AddedOn { get; set; } // 28.04.2017 20:06:25:0255

        public bool Visible { get; set; } // false

        public string ImagePath { get; set; }   // 2017/04/First_Album/Original/this_is_photo.jpg
        public string ThumbPath { get; set; }   // 2017/04/First_Album/Thumbnail/this_is_photo.jpg
        public string ResizedPath { get; set; } // 2017/04/First_Album/Resized/this_is_photo.jpg

        public virtual Album Album { get; set; }
    }
}