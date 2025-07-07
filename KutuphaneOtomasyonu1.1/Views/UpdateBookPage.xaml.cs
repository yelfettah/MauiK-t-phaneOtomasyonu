using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;
using KutuphaneOtomasyonu.Views; 

namespace KutuphaneOtomasyonu.Views;

public partial class UpdateBookPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly Book _book;

    public UpdateBookPage(DatabaseService db, Book book)
    {
        InitializeComponent();
        _db = db;
        _book = book;
        titleEntry.Text = _book.Title;
        authorEntry.Text = _book.Author;
        isbnEntry.Text = _book.ISBN;
        yearEntry.Text = _book.PublishedYear.ToString();
    }
    private async void OnSaveClicked(object sender, EventArgs e)
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

        _book.Title = title;
        _book.Author = author;
        _book.ISBN = isbn;
        _book.PublishedYear = year;

  
        _book.UpdatedBy = App.CurrentUser?.Username ?? "Sistem";
        _book.UpdatedDate = DateTime.Now;

        await _db.UpdateBook(_book);
        await DisplayAlert("Başarılı", "Kitap güncellendi.", "Tamam");
        await Navigation.PopAsync();
    }

}
