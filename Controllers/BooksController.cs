using AutoMapper;
using Books.API.Filters;
using Books.API.Models;
using Books.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository booksRepository,IMapper mapper)
        {
            _repository = booksRepository
                ?? throw new ArgumentNullException(nameof(booksRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            var bookEntities = await _repository.GetBooksAsync();
            return Ok(bookEntities);
        }
        [HttpGet]
        [Route("{id}",Name ="GetBook")]
        [BookWithCoversResultFilter]
        public async Task<IActionResult> GetBookById(string id)
        {
            var book = await _repository.GetBookByIdAsync(id);
            if(book==null)
                return NotFound();
            var bookCovers = await _repository.GetBookCoversAsync(id);
            
            
            return Ok((book:book,bookCovers:bookCovers));
        }
        [HttpPost]
        [BookWithCoversResultFilter]
        public async Task<IActionResult> CreateBook(BookForCreation bookForCreation)
        {
            var bookEntity = _mapper.Map<Entities.Book>(bookForCreation);
            _repository.AddBook(bookEntity);
            await _repository.SaveChangesAsync();
            await _repository.GetBookByIdAsync(bookEntity.Id);
            return CreatedAtRoute("GetBook", new {id=bookEntity.Id},bookEntity);
        }


    }
}
