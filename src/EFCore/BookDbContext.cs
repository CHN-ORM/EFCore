using Microsoft.EntityFrameworkCore;
using EFCore.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => builder.UseSqlite("Data Source=books.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Library)
                .WithMany(l => l.Books);

            modelBuilder.Entity<Library>()
                .HasMany(l => l.Books);
        }
    }
}