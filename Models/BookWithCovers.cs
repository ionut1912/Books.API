namespace Books.API.Models
{
    public class BookWithCovers:Book
    {
        public IEnumerable<BookCover> bookCovers { get; set; }=new List<BookCover>();  
    }
}
