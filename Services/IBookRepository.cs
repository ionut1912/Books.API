using Books.API.Entities;
using Books.API.ExternalModels;

namespace Books.API.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(string id);
        void AddBook(Entities.Book bookToAdd);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Entities.Book>> GetBookListByIdAsync(IEnumerable<string> id);
        Task<BookCover> GetBookCoverAsync(string id);
        Task<IEnumerable<BookCover>> GetBookCoversAsync(string bookId);
    }
}
