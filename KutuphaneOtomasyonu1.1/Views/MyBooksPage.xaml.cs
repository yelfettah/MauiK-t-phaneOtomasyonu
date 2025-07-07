using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;
using KutuphaneOtomasyonu.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KutuphaneOtomasyonu.Views
{
    public partial class MyBooksPage : ContentPage
    {
        private readonly DatabaseService _db;
        private List<Book> myBooks;

        public MyBooksPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBooks();
        }

        private async Task LoadBooks()
        {
            var user = App.CurrentUser;
            if (user != null)
            {
                var books = await _db.GetBooks();
                myBooks = books.Where(b => b.CreatedBy == user.Username).ToList();
                myBooksCollectionView.ItemsSource = myBooks;

                if (!myBooks.Any())
                {
                    await DisplayAlert("Bilgi", "Henüz hiç kitabınız yok.", "Tamam");
                }
            }
            else
            {
                await DisplayAlert("Hata", "Kullanıcı bilgisi bulunamadı", "Tamam");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var book = (Book)button.BindingContext;

            bool confirm = await DisplayAlert("Sil", $"'{book.Title}' adlı kitabı silmek istiyor musunuz?", "Evet", "Hayır");
            if (confirm)
            {
                await _db.DeleteBook(book.Id);
                await LoadBooks();
            }
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var book = (Book)button.BindingContext;

            await Navigation.PushAsync(new UpdateBookPage(_db, book));
        }

        private async void OnDeleteSwipe(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var book = swipeItem?.CommandParameter as Book;
            if (book == null) return;
            bool confirm = await DisplayAlert("Kitap Sil", $"{book.Title} kitabını silmek istediğinize emin misiniz?", "Evet", "Hayır");
            if (confirm)
            {
                await _db.DeleteBook(book.Id);
                await LoadBooks();
            }
        }
    }
}
