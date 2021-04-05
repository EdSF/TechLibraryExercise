using System.Buffers;
using AutoMapper;
using TechLibrary.Contracts.Responses;
using TechLibrary.Domain;

namespace TechLibrary.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Book, BookResponse>().ForMember(x => x.Descr, opt => opt.MapFrom(src => src.ShortDescr))
                .ReverseMap().ForMember(x =>x.ShortDescr, opt => opt.MapFrom(s => s.Descr));
            
            CreateMap<PageList<Book>, PagedBookResponse>()
                .ForMember(x => x.Books, opt => opt.MapFrom(src => src.Items));
        }
    }
}