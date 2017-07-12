using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWorkshop.Models
{
    public class Album
    {
        public Album()
        {
            Images = new List<Image>();
        }

        public int AlbumId { get; set; }

        public string Title { get; set; }
        [Display(Name = "Url slug")]
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Occasion { get; set; }
        public bool Published { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified on")]
        public DateTime ModifiedOn { get; set; }


        public string PreviewImagePath { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}