using SQLite;

namespace KutuphaneOtomasyonu.Models;

public class Book
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Author { get; set; }
    public int PublishedYear { get; set; }

    public string ISBN { get; set; } 

    public bool IsAvailable { get; set; } = true;
    public int MemberId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

}

