using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Services
{
    //public interface IDal
    //{
    //    Task<List<Post>> GetPostAsync(int take, int skip);
    //    Task<Post> GetPostBySlugAsync(string slug);
    //}

    //public class Dal : IDal
    //{
    //    public Task<List<Post>> GetPostAsync(int take, int skip = 0)
    //    {
    //        using (ApplicationDbContext Ctx = new ApplicationDbContext())
    //        {
    //            return Ctx.Posts.Take(take).Skip(skip).AsNoTracking().ToListAsync();
    //        }

    //    }

    //    Task<Post> IDal.GetPostBySlugAsync(string slug)
    //    {
    //        using (ApplicationDbContext Ctx = new ApplicationDbContext())
    //        {
    //            return Ctx.Posts.SingleOrDefaultAsync(m => m.Slug == slug);
    //        }
    //    }
    //}
}
