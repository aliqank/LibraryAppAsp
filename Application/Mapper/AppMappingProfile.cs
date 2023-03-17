using Application.Dto;
using Application.Dto.Book;
using Application.Dto.BorrowHistory;
using Application.Dto.User;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;

namespace Application.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<BorrowHistoryCreateDto, BorrowHistory>()
            .ForMember(p => p.DueDate,
                o => o.MapFrom(
                    (s, p) => p.BorrowDate.AddDays(s.BorrowDurationInDays)));
        CreateMap<BorrowReturnDto, BorrowHistory>();
        CreateMap<BorrowHistory, BorrowReadDto>();

        CreateMap<BookCreateDto, Book>();
        CreateMap<BookStatusUpdateDto, Book>();
        CreateMap<Book, BookReadDto>();
        CreateMap<BookReadDto, Book>();
        CreateMap<BookUpdateDto, Book>();
        CreateMap<BookReadDto, BookUpdateDto>();
        CreateMap<Book, BookUpdateDto>();
        CreateMap<BookUpdateDto, BookReadDto>();

        CreateMap<UserCreateDto, User>()
            .ForMember(p => p.Rating, 
                o => o.MapFrom(s=> RatingType.Neutral));
        CreateMap<UserUpdateDto, User>();
        CreateMap<UserReadDto, User>();
        CreateMap<UserUpdateDto, UserReadDto>();
        CreateMap<UserReadDto, UserUpdateDto>();
        CreateMap<User, UserReadDto>();
        CreateMap<User, UserUpdateDto>();
    }
    
}

