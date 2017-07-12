using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.ViewModels
{
    public class PostViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 64 символов")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Url slug")]
        [StringLength(44, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 44 символов")]
        public string UrlSlug { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Content")]
        public string Description { get; set; }

        [Required]
        public bool Published { get; set; }

        public string ImagePath { get; set; }
        //public byte[] ImageData { get; set; }

        //public string ImageMimeType { get; set; }
        [RegularExpression(@"([\w\d]{3,}\s?)*", ErrorMessage = "Only words(min length = 3) with letters or numbers allowed")]
        public string Tags { get; set; }
    }
}