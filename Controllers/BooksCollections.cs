using AutoMapper;
using Books.API.Filters;
using Books.API.ModelBinders;
using Books.API.Models;
using Books.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BooksResultFilter]
    public class BooksCollections : ControllerBase
    {
        private readonly IBookRepository? _repo;
        private readonly IMapper _mapper;

        public BooksCollections(IBookRepository bookRepo,IMapper mapper)
        {
            _repo=bookRepo ??throw new ArgumentNullException(nameof(bookRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("({bookIds})",Name ="GetBookCollection")]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))]IEnumerable<string> bookIds)
        {
            var bookEntities = await _repo.GetBookListByIdAsync(bookIds);
            if(bookIds.Count()!=bookEntities.Count())
            {
                return NotFound();
            }
            return Ok(bookEntities);

        }
        [HttpPost]
        public async Task<IActionResult> CreateBookCollection(IEnumerable<BookForCreation> bookForCreation)
        {
            var bookEntities = _mapper.Map<IEnumerable<Entities.Book>>(bookForCreation);
            foreach(var bookEntity in bookEntities)
            {
                _repo.AddBook(bookEntity);
            }
            await _repo.SaveChangesAsync();
            var booksToReturn = await _repo.GetBookListByIdAsync(bookEntities.Select(b => b.Id).ToList());
           var bookIds=string.Join(",",booksToReturn.Select(b=>b.Id));
            return CreatedAtRoute("GetBookCollection", new { bookIds }, booksToReturn);
        }
    }
}
