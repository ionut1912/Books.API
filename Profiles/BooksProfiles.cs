using AutoMapper;

namespace Books.API.Profiles
{
    public class BooksProfiles:Profile
    {
        public BooksProfiles()
        {
            CreateMap<Entities.Book, Models.Book>()
              .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                 $"{src.Author.FirstName} {src.Author.LastName}"));
            CreateMap<Models.BookForCreation, Entities.Book>();
            CreateMap<Entities.Book,Models.BookWithCovers>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                 $"{src.Author.FirstName} {src.Author.LastName}"));
            CreateMap<IEnumerable<ExternalModels.BookCover>,Models.BookWithCovers>()
                .ForMember(dest => dest.bookCovers, opt => opt.MapFrom(src =>
                 src));
            CreateMap<ExternalModels.BookCover, Models.BookCover>();
        }
    }
}
