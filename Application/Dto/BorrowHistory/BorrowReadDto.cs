namespace Application.Dto.BorrowHistory;

public class BorrowReadDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnedDate { get; set; }
    public DateTime DueDate { get; set; }
    public int BorrowDurationInDays { get; set; }
    public bool IsReturned { get; set; }
}