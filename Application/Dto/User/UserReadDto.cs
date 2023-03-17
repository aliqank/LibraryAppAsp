using Application.Dto.Book;
using Application.Dto.BorrowHistory;
using Domain.Enum;

namespace Application.Dto.User;

public class UserReadDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public RatingType Rating { get; set; }
    public int Limit { get; set; } = 1;
    public ICollection<BookReadDto>? Books { get; set; }
    public ICollection<BorrowReadDto>? BorrowHistory { get; set; }
}