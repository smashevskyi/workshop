using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkshop.DAL.Abstract
{
    public interface IAlbumRepository : IDisposable
    {
        IEnumerable<Album> GetAlbums(Expression<Func<Album, bool>> filter = null, Func<IQueryable<Album>, IOrderedQueryable<Album>> orderBy = null, bool includeProp = false, int? page = null, int? size = null);
        Album GetAlbumById(int albumId);
        Album InsertAlbum(Album album);
        void DeleteAlbum(int albumId);
        void UpdateAlbum(Album album);
        int CountAlbums(Expression<Func<Album, bool>> filter = null);
        IList<ArchiveEntry> GetArchive();

        Image GetImageById(int id);
        void UpdateImage(Image image);
        //void UpdateImageInfo(Image image);
        void DeleteImage(Image image);

        void Save();
    }
}
