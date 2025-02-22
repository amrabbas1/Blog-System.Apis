using BlogSystem.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Data
{
    public class BlogDbContext : IdentityDbContext<User>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogPost>()
                .HasOne(bp => bp.Author)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(bp => bp.AuthorId);

            modelBuilder.Entity<BlogPost>()
                .HasOne(bp => bp.Category)
                .WithMany(c => c.BlogPosts)
                .HasForeignKey(bp => bp.CategoryId);

            modelBuilder.Entity<BlogPost>()
                .HasMany(bp => bp.Tags)
                .WithMany(t => t.BlogPosts)
                .UsingEntity(j => j.ToTable("BlogPostTags"));

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.BlogPost)
                .WithMany(bp => bp.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<User>()
                        .Property(u => u.Role)
                        .HasConversion(URole => URole.ToString(), URole => (UserRole)Enum.Parse(typeof(UserRole), URole));// Store enum as a string

            modelBuilder.Entity<BlogPost>()
                        .Property(b => b.Status)
                        .HasConversion(BStatus => BStatus.ToString(), BStatus => (PostStatus)Enum.Parse(typeof(PostStatus),BStatus));

        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
