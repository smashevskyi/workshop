using MyWorkshop.DAL.Abstract;
using MyWorkshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWorkshop.DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private MWContext context = new MWContext();
        private IPostRepository postRepository;
        private IAlbumRepository albumRepository;

        public IPostRepository PostRepository
        {
            get
            {
                if (this.postRepository == null)
                {
                    this.postRepository = new PostRepository(context);
                }
                return postRepository;
            }
        }

        public IAlbumRepository AlbumRepository
        {
            get
            {
                if (this.albumRepository == null)
                {
                    this.albumRepository = new AlbumRepository(context);
                }
                return albumRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
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