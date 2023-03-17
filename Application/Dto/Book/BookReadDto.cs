namespace Application.Dto.Book;

public class BookReadDto
{
    public string Title { get; set; }
    public bool IsAvailable { get; set; } = true;
    public long UserId { get; set; }
}