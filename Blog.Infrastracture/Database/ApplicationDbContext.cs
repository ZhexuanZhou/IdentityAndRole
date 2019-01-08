using System;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastracture.Database
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PostTag>().HasKey(pt=>new {pt.PostId, pt.TagId});
        }

        public DbSet<AppUser> AppUsers{get;set;}
        public DbSet<AppRole> AppRoles{get;set;}
        public DbSet<Post> Posts{get;set;}
        public DbSet<Tag> Tags{get;set;}
        public DbSet<Comment> Comments{get;set;}
        public DbSet<PostTag> PostTags{get;set;}

        public static class RoleName
        {
            public const string User = "User";
            public const string SuperUser = "SuperUser";
        }
        public static class PolicyName
        {
            public const string Users = "Users";
            public const string SuperUsers = "SuperUsers";
        }

        public static class ClaimName
        {
            public const string User = "User";
            public const string SuperUser = "SuperUser";
        }
    }
}