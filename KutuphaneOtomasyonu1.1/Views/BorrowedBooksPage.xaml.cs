using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;
using Microsoft.Maui.Controls;

namespace KutuphaneOtomasyonu.Views
{
    public partial class BorrowedBooksPage : ContentPage
    {
        private readonly DatabaseService _db;

        public BorrowedBooksPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadBorrowedBooks();
        }

        private async Task LoadBorrowedBooks()
        {
            var borrowings = await _db.GetBorrowedBooksByUser(App.CurrentUser.Id);
            var borrowedBooksWithDetails = new List<BorrowingWithDetails>();
            foreach (var b in borrowings)
            {
                var book = await _db.GetBookById(b.BookId);
                borrowedBooksWithDetails.Add(new BorrowingWithDetails
                {
                    Id = b.Id,
                    BookId = b.BookId,
                    UserId = b.UserId,
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate,
                    BookTitle = book?.Title ?? "Bilinmeyen Kitap",
                    BookAuthor = book?.Author ?? "Bilinmeyen Yazar"
                });
            }
            BorrowedBooksCollectionView.ItemsSource = borrowedBooksWithDetails;
        }

        private async void OnReturnBookClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var borrowing = button?.CommandParameter as BorrowingWithDetails;

            if (borrowing == null) return;

            if (borrowing.UserId != App.CurrentUser.Id)
            {
                await DisplayAlert("Hata", "Sadece kendi ödünç aldığınız kitapları iade edebilirsiniz.", "Tamam");
                return;
            }

            bool confirmed = await DisplayAlert("İade Onayı", $"\"{borrowing.BookTitle}\" kitabını iade etmek istediğinize emin misiniz?", "Evet", "Hayır");

            if (confirmed)
            {
                await _db.ReturnBook(borrowing.Id);
                await DisplayAlert("Başarılı", "Kitap iade edildi.", "Tamam");
                await LoadBorrowedBooks();
            }
        }

        public class BorrowingWithDetails : Borrowing
        {
            public string BookTitle { get; set; }
            public string BookAuthor { get; set; }
            public bool IsBorrowedByCurrentUser => UserId == App.CurrentUser.Id;
        }
    }
}
