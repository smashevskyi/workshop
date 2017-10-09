using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class ManageViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Album> Albums { get; set; }
    }
}