using Books.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.API.Contexts
{
    public class BooksContext:DbContext
    {
        public DbSet<Book>? Books { get; set; }
        public BooksContext(DbContextOptions<BooksContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author()
                {
                    Id ="d288-34dfdsf-2324-fsdf",
                    FirstName = "George",
                    LastName = "RR Martin"
                },
                new Author()
                {
                    Id="d23423543-edffsf4-4fdfg1",
                    FirstName="Stephen",
                    LastName="Fry"
                }
                );
            modelBuilder.Entity<Book>().HasData(
                
                new Book()
                {
                    Id="w3e34-23ff-432ff",
                    AuthorId= "d288-34dfdsf-2324-fsdf",
                    Title= "The story of my live",
                    Description= "A very interesting book"
                },
                new Book()
                {
                    Id = "w5e34-23ff-432ff",
                    AuthorId ="d288-34dfdsf-2324-fsdf",
                    Title = "The story of my  girlfriend live",
                    Description = "A very interesting book"
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
