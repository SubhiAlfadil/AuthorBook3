using Microsoft.EntityFrameworkCore;

namespace AuthorBook3.Models
{
    public class DBContexta : DbContext
    {
        public DBContexta(DbContextOptions<DBContexta> options)
       : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookType> BookTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
             .HasOne<Author>(s => s.Author)
             .WithMany(g => g.Books)
             .HasForeignKey(s => s.AuthorId);

            modelBuilder.Entity<Book>()
             .HasOne<BookType>(s => s.BookType)
             .WithMany(g => g.Books)
             .HasForeignKey(s => s.BookTypeId);
        }
    }
}
