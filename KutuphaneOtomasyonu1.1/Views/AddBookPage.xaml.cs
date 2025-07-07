using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;

namespace KutuphaneOtomasyonu.Views;

public partial class AddBookPage : ContentPage
{
    private readonly DatabaseService _db;

    public AddBookPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }
     private async void OnAddBookClicked(object sender, EventArgs e)
{
    string title = titleEntry.Text?.Trim();
    string author = authorEntry.Text?.Trim();
    string isbn = isbnEntry.Text?.Trim();
    bool isYearValid = int.TryParse(yearEntry.Text, out int year);

    if (string.IsNullOrEmpty(title) || title.Length < 2 || title.Length > 100)
    {
        await DisplayAlert("Hata", "Başlık 2-100 karakter arası olmalıdır.", "Tamam");
        return;
    }
    if (string.IsNullOrEmpty(author) || author.Length < 2 || author.Length > 50)
    {
        await DisplayAlert("Hata", "Yazar 2-50 karakter arası olmalıdır.", "Tamam");
        return;
    }
    if (string.IsNullOrEmpty(isbn) || isbn.Length < 5 || isbn.Length > 20)
    {
        await DisplayAlert("Hata", "ISBN 5-20 karakter arası olmalıdır.", "Tamam");
        return;
    }
    if (!isYearValid || year < 1800 || year > DateTime.Now.Year)
    {
        await DisplayAlert("Hata", $"Yıl 1800-{DateTime.Now.Year} arası olmalıdır.", "Tamam");
        return;
    }

    var newBook = new Book
    {
        Title = title,
        Author = author,
        ISBN = isbn,
        PublishedYear = year,
        IsAvailable = true,
        MemberId = App.CurrentUser?.Id ?? 0,
        CreatedBy = App.CurrentUser?.Username ?? "Sistem",
        CreatedDate = DateTime.Now,
        UpdatedBy = null,
        UpdatedDate = null
    };

    _db.AddBook(newBook);

    await DisplayAlert("Başarılı", "Kitap başarıyla eklendi!", "Tamam");
    await Navigation.PopAsync();
}


   /* private async void OnAddBookClicked(object sender, EventArgs e)
    {
        string title = titleEntry.Text?.Trim();
        string author = authorEntry.Text?.Trim();
        string isbn = isbnEntry.Text?.Trim();
        bool isYearValid = int.TryParse(yearEntry.Text, out int year);

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || !isYearValid)
        {
            await DisplayAlert("Hata", "Lütfen tüm alanları doğru şekilde doldurun.", "Tamam");
            return;
        }

        var newBook = new Book
        {
            Title = title,
            Author = author,
            ISBN = isbn,
            PublishedYear = year,
            IsAvailable = true,
            MemberId = 0,         // Üye ile ilişkilendirme yok, 0 veya -1 olarak bırakabiliriz
            CreatedBy = App.CurrentUser?.Username ?? "Unknown",
            CreatedDate = DateTime.Now,
            UpdatedBy = null,
            UpdatedDate = null
        };

        _db.AddBook(newBook);

        await DisplayAlert("Başarılı", "Kitap başarıyla eklendi!", "Tamam");

        await Navigation.PopAsync();
    }*/
}
