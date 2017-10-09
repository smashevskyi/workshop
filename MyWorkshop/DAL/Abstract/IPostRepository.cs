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
    public interface IPostRepository : IDisposable
    {
        IEnumerable<Post> GetPosts(Expression<Func<Post, bool>> filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> sortBy = null, int? page = null, int? size = null);
        Post GetPostById(int postId);
        Post InsertPost(Post post);
        void DeletePost(int postId);
        void UpdatePost(Post post);
        int CountPosts(Expression<Func<Post, bool>> filter);
        IOrderedEnumerable<Post> GetPostByTagId(int tagId);
        IList<ArchiveEntry> GetArchive();

        Tag GetTagByName(string tagName);
        Tag InsertTag(string tagName);
        Dictionary<string, int> GetTagsCounted();

        void Save();
    }
}
