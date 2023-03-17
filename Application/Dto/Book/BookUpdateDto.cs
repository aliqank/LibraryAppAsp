namespace Application.Dto.Book;

public class BookUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsAvailable { get; set; } = true;
    
}