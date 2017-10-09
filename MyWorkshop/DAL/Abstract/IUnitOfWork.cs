using MyWorkshop.DAL.Concrete;

namespace MyWorkshop.DAL.Abstract
{
    public interface IUnitOfWork
    {
        IAlbumRepository AlbumRepository { get; }
        IPostRepository PostRepository { get; }

        void Dispose();
        void Save();
    }
}