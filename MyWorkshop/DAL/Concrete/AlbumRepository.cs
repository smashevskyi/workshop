using MyWorkshop.DAL.Abstract;
using MyWorkshop.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWorkshop.Models;
using System.Linq.Expressions;
using System.Data.Entity;
using MyWorkshop.ViewModels;

namespace MyWorkshop.DAL.Concrete
{
    public class AlbumRepository : IAlbumRepository
    {
        internal MWContext context;
        internal DbSet<Album> dbSet;

        public AlbumRepository(MWContext context)
        {
            this.context = context;
            this.dbSet = context.Set<Album>();
        }

        public void DeleteAlbum(int albumId)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(Image image)
        {
            context.Images.Remove(image);
        }

        public Album GetAlbumById(int albumId)
        {
            return context.Albums.Where(x => x.AlbumId == albumId).Include(x => x.Images).FirstOrDefault();
        }

        public IEnumerable<Album> GetAlbums(Expression<Func<Album, bool>> filter = null, 
                                            Func<IQueryable<Album>, IOrderedQueryable<Album>> orderBy = null, 
                                            bool includeProp = false,
                                            int? page = default(int?), 
                                            int? size = default(int?))
        {
            IQueryable<Album> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProp)
            {
                query = query.Include("Images");
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
                query = query.OrderByDescending(a => a.AlbumId);
            }

            if (page != null && size != null)
            {
                query = query.Skip((Convert.ToInt32(page) - 1) * Convert.ToInt32(size)).Take(Convert.ToInt32(size));
            }

            return query.ToList();
        }

        public int CountAlbums(Expression<Func<Album, bool>> filter = null)
        {
            if (filter != null)
            {
                IQueryable<Album> records = dbSet;
                return records.Where(filter).Count();
            }
            return (dbSet as IQueryable<Album>).Count();
        }

        public Image GetImageById(int id)
        {
            return context.Images.Find(id);
        }

        public Album InsertAlbum(Album album)
        {
            return context.Albums.Add(album);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateAlbum(Album album)
        {
            context.Entry(album).State = EntityState.Modified;
        }

        public void UpdateImage(Image image)
        {
            context.Entry(image).State = EntityState.Modified;
        }



        public IList<ArchiveEntry> GetArchive()
        {
            return context.Albums
                .GroupBy(o => new
                {
                    Month = o.CreatedOn.Month,
                    Year = o.CreatedOn.Year
                })
                .Select(g => new ArchiveEntry
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = g.Count()
                })
                .OrderByDescending(a => a.Year)
                .ThenByDescending(a => a.Month)
                .ToList();
        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}