using MyWorkshop.DAL.Abstract;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MyWorkshop.DAL.Concrete
{
    public class PostRepository : IPostRepository
    {
        internal MWContext context;
        internal DbSet<Post> dbSet;

        public PostRepository(MWContext context)
        {
            this.context = context;
            this.dbSet = context.Set<Post>();
        }

        public IEnumerable<Post> GetPosts(Expression<Func<Post, bool>> filter = null,
                                            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null,
                                            int? page = null,
                                            int? size = null)
        {
            IQueryable<Post> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
                query = query.OrderByDescending(p => p.Id);
            }

            if (page != null && size != null)
            {
                query = query.Skip((Convert.ToInt32(page) - 1) * Convert.ToInt32(size)).Take(Convert.ToInt32(size));
            }

            return query.ToList();
        }


        public Post GetPostById(int id)
        {
            return context.Posts.Find(id);
        }

        public Post InsertPost(Post post)
        {
            return context.Posts.Add(post);
        }

        public void DeletePost(int postID)
        {
            Post post = context.Posts.Find(postID);
            context.Posts.Remove(post);
        }
        public void Delete(Post entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public void UpdatePost(Post post)
        {
            context.Entry(post).State = EntityState.Modified;
        }

        public int CountPosts(Expression<Func<Post, bool>> filter)
        {
            if (filter != null)
            {
                IQueryable<Post> records = dbSet;
                return records.Where(filter).Count();
            }
            return (dbSet as IQueryable<Post>).Count();
        }

        public Tag GetTagByName(string tagName)
        {
            return context.Tags.Where(t => t.Name == tagName).SingleOrDefault();
        }

        public Tag InsertTag(string tagName)
        {
            return context.Tags.Add(new Tag() { Name = tagName });
        }
        public IOrderedEnumerable<Post> GetPostByTagId(int tagId)
        {
            return context.Tags.Include(x => x.Posts).FirstOrDefault(x => x.Id == tagId).Posts.Where(p => p.Published).OrderByDescending(p => p.Id);
        }

        public Dictionary<string, int> GetTagsCounted()
        {
            return context.Tags.Include(x => x.Posts).Where(x => x.Posts.Count > 0).OrderBy(x => x.Name).ToDictionary(x => x.Name, x => x.Posts.Count);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public IList<ArchiveEntry> GetArchive()
        {
            return context.Posts
                .GroupBy(o => new
                {
                    Month = o.PostedOn.Month,
                    Year = o.PostedOn.Year
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