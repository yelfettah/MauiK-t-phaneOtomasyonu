using SQLite;

namespace KutuphaneOtomasyonu.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }  // Veritabanı için otomatik artan ID
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserType { get; set; } // Picker için
        public DateTime BirthDate { get; set; } 


    }
}
