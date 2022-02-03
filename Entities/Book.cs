using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.API.Entities
{   [Table("Books")]  
    public class Book
    {
       [Key]
       public string? Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Title { get; set; }
        [MaxLength(2500)]
        public string ? Description { get; set; }
        public string ? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}

