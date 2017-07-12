using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class AlbumViewModel
    {
        //public AlbumViewModel() { }
        [Key]
        public int AlbumId { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 64 символов")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Url slug")]
        [StringLength(44, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 44 символов")]
        public string Slug { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Максимальная длина 256 символов")]
        public string Description { get; set; }

        //public string PreviewImagePath { get; set; }
        [MaxLength(24, ErrorMessage = "Максимальная длина 24 символов")]
        public string Occasion { get; set; }

        [Required]
        public bool Published { get; set; }

    }
}