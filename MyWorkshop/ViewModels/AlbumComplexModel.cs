using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class AlbumComplexModel
    {
        public IList<Album> Albums { get; set; }
        public IList<ArchiveEntry> ArchiveEntries { get; set; }
    }
}