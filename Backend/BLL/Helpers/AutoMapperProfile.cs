using AutoMapper;
using BLL.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /*CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.BookInfo.Author.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.BookInfo.Title.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.BookInfo.Picture.Url))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.BookInfo.Publisher.Name))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BookInfo.BookGenre.Select(bt => bt.Genre.Name).ToList()));

            *//*CreateMap<BLL.Models.BookDto, Domain.Entities.Book>()
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.BookTopics, opt => opt.Ignore())
                .ForMember(dest => dest.Position, opt => opt.Ignore())
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();*//*
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookInfo.AuthorId, opt => opt.Ignore())
                .ForMember(dest => dest.BookInfo.PublisherId, opt => opt.Ignore())
                .ForMember(dest => dest.BookInfo.Author, opt => opt.Ignore())
                .ForMember(dest => dest.BookInfo.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.Row, opt => opt.Ignore())
                .ForMember(dest => dest.BookInfo.BookGenre, opt => opt.Ignore())
                .ForMember(dest => dest.Reservations, opt => opt.Ignore());
*/
            CreateMap<User, UserDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));


        }
    }
}
