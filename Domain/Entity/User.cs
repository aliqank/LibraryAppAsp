using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enum;

namespace Domain.Entity;

public class User
{ 
    public long Id { get; set; }
    [Column(TypeName = "varchar(20)")]
    public RatingType Rating { get; set; }
    public string Name { get; set; }
    public int Limit { get; set; } = 1;
    public ICollection<Book>? Books { get; set; }
    public ICollection<BorrowHistory>? BorrowHistory { get; set; }
    
}