namespace Application.Dto.BorrowHistory;

public class BorrowHistoryCreateDto
{
    public long UserId { get; set; }
    public int BookId { get; set; }
    public int BorrowDurationInDays { get; set; }
}