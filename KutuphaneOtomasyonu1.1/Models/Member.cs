// Bu dosya artık kullanılmıyor. User modelini kullanıyoruz.

using SQLite;

namespace KutuphaneOtomasyonu.Models;

public class Member
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
