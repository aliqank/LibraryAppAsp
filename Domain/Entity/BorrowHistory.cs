using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entity;

public class BorrowHistory
{
    public long Id { get; set; }
    public long UserId { get; set; }
    [ForeignKey("UserId")]
    [JsonIgnore]
    public User? User { get; set; }
    [ForeignKey("BookId")]
    public long BookId { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnedDate { get; set; }
    public DateTime DueDate { get; set; }
    public int BorrowDurationInDays { get; set; }
    public bool IsReturned { get; set; }
    public bool IsUpdated { get; set; }
}