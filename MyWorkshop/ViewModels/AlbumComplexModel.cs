using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWorkshop.ViewModels
{
    public class AlbumComplexModel
    {
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<ArchiveEntry> ArchiveEntries { get; set; }
    }
}