using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HelloApp.Model
{
    public partial class HelloAppContext : DbContext
    {
        public HelloAppContext()
        {
        }

        public HelloAppContext(DbContextOptions<HelloAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Post> Post { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       // {
			//if (!optionsBuilder.IsConfigured)
			//{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
			//optionsBuilder.UseSqlServer("Server=.;Database=HelloApp;User=sa;Password=sa;Integrated Security=False");

			//}
		//}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

			//modelBuilder.Entity<Blog>()
			//	.ToTable("a", schema: "dbo");

			modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Blog)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blog_Category");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.BlogId);

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Blog");
            });
        }
    }
}
