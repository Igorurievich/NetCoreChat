﻿using App.Comments.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Comments.Data
{
    public class CommentsContext : DbContext
    {
		public CommentsContext()
		{
		}

		public CommentsContext(DbContextOptions<CommentsContext> options) : base(options)
        {
			
		}

        public DbSet<Comment> Comments { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<CommentData> CommentsData { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
			modelBuilder.Entity<Comment>().ToTable("Comment");
			modelBuilder.Entity<CommentData>().ToTable("CommentData");
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}
