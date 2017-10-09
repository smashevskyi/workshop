using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class SidebarComplexModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<ArchiveEntry> ArchiveEntries { get; set; }
    }
}