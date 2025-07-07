public class BookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int PublishedYear { get; set; }
    public bool IsAvailable { get; set; }
    public int MemberId { get; set; }
    public string BorrowedByUserName { get; set; }  
    public bool IsBorrowedByCurrentUser { get; set; }  
    public string AddedByUserName { get; set; } 
     public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
