using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWorkshop.Models
{
        
    public class Post
    {
        public Post()
        {
            Tags = new List<Tag>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Url slug")]
        public string UrlSlug { get; set; }

        public bool Published { get; set; }

        [Display(Name = "Posted On")]
        public DateTime PostedOn { get; set; }
        
        public string ImagePath { get; set; }

        //public byte[] ImageData { get; set; }

        //public string ImageMimeType { get; set; }

        public virtual IList<Tag> Tags { get; set; }

    }
}
