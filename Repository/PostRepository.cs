using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreServices.Data;
using CoreServices.Models;
using CoreServices.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CoreServices.Repository
{
    public class PostRepository : IPostRepository
    {
        BlogDbContext _context;
        public PostRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<List<PostViewModel>> GetPosts()
        {
            return await (from p in _context.Post
                from c in _context.Category
                where p.CategoryId == c.Id
                select new PostViewModel
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = c.Name,
                    CreatedDate = p.CreatedDate
                }).ToListAsync();
        }

        public async Task<PostViewModel> GetPost(int? postId)
        {
            return await (from p in _context.Post
                from c in _context.Category
                where p.PostId == postId
                select new PostViewModel
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = c.Name,
                    CreatedDate = p.CreatedDate
                }).FirstOrDefaultAsync();
        }

        public async Task<int> AddPost(Post post)
        {
            await _context.Post.AddAsync(post);
            await _context.SaveChangesAsync();

            return post.PostId;
        }

        public async Task DeletePost(int? postId)
        {
            var post = await _context.Post.FirstOrDefaultAsync(x => x.PostId == postId);

            if (post != null)
            {
                //Delete that post
                _context.Post.Remove(post);

                //Commit the transaction
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePost(Post post)
        {
            //Delete that post
            _context.Post.Update(post);

            //Commit the transaction
            await _context.SaveChangesAsync();
        }
    }
}
