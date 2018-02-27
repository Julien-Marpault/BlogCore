using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogCore.Models;

namespace BlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDisposable
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(Startup.ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<PostContent> PostContents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<LinkCategoryToPost> LinksCategoriesToPosts { get; set; }
    }
}
