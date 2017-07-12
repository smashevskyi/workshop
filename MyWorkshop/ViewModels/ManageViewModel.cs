using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class ManageViewModel
    {
        public IList<Post> Posts { get; set; }
        public IList<Album> Albums { get; set; }
    }
}