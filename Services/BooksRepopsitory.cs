using Books.API.Contexts;
using Books.API.Entities;
using Books.API.ExternalModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Books.API.Services
{
    public class BooksRepopsitory : IBookRepository,IDisposable
    {
        private  BooksContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;

        public BooksRepopsitory(BooksContext booksContext,IHttpClientFactory httpClientFactory)
        {
            _context = booksContext ?? throw new ArgumentNullException(nameof(booksContext));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }   

        public async Task<Book> GetBookByIdAsync(string id)
        {
           return await _context.Books.
                        Include(b => b.Author)
                        .FirstOrDefaultAsync(b => b.Id == id);


        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await  _context.Books.Include(b=>b.Author).ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }

            }
        }

        public void AddBook(Book bookToAdd)
        {

            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }
            _context.Add(bookToAdd);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<IEnumerable<Book>> GetBookListByIdAsync(IEnumerable<string> id)
        {

            return await _context.Books.Where(b => id.Contains(b.Id)).Include(b => b.Author).ToListAsync();
        }

        public async Task<BookCover> GetBookCoverAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response =await httpClient.GetAsync($"http://localhost:34828/api/bookcovers/{id}");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
            }
            return null;
        }

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(string bookId)
        {
            var  httpClient= _httpClientFactory.CreateClient();
            var bookCovers= new List<BookCover>();
            _cancellationTokenSource=new CancellationTokenSource();
            
            var bookCoversUrls = new[]
            {
                $"http://localhost:7157/api/bookcovers/{bookId}-dummycover1",

                $"http://localhost:7157/api/bookcovers/{bookId}-dummycover2",

                $"http://localhost:7157/api/bookcovers/{bookId}-dummycover3",

                $"http://localhost:7157/api/bookcovers/{bookId}-dummycover4",

                $"http://localhost:7157/api/bookcovers/{bookId}-dummycover5"
            };
            var downloadCoversQuerry = from bookCoverUrl
                                       in bookCoversUrls
                                       select DownloadBookCoverAsync(httpClient, bookCoverUrl,_cancellationTokenSource.Token);
            var downloadBookCoverTask = downloadCoversQuerry.ToList();
            try
            {
                return await Task.WhenAll(downloadBookCoverTask);
            }
            catch (OperationCanceledException  ex)
            {
                throw;

            }
          
                
        }
        public async Task<BookCover> DownloadBookCoverAsync(HttpClient httpClient,string bookCoverUrl,CancellationToken cancellationToken)
        {
            var response= await httpClient.GetAsync(bookCoverUrl);
            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                return bookCover;
            }
            _cancellationTokenSource.Cancel();
            return null;
        }
    }
}
