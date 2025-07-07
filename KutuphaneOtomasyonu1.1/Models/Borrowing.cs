using SQLite;

namespace KutuphaneOtomasyonu.Models;

public class Borrowing
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int UserId { get; set; }      
    public int BookId { get; set; }    
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
     public string BookTitle { get; set; }
     public string CreatedBy { get; set; }
     public DateTime CreatedDate { get; set; }
     public string? UpdatedBy { get; set; }
     public DateTime? UpdatedDate { get; set; }
       

}
