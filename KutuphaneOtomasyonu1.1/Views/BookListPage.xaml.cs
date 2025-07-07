using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;
using System.Linq;

namespace KutuphaneOtomasyonu.Views;

public partial class BookListPage : ContentPage
{
    private readonly DatabaseService _db;
    private List<BookViewModel> _books;

    public BookListPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        ToolbarItems.Add(new ToolbarItem("Çıkış", null, async () =>
        {
            App.CurrentUser = null;
            App.CurrentUserr = null;
            await Navigation.PushAsync(new LoginPage(_db));
            Navigation.RemovePage(this);
        }));
        LoadBooks();
    }

    private async void LoadBooks()
    {
        _books = await _db.GetAllBooksWithBorrowInfo(App.CurrentUser.Id);
        BooksCollectionView.ItemsSource = _books;
    }

    private async void OnAddBookClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBookPage(_db));
    }

    private async void OnUpdateBookClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var bookVM = button?.CommandParameter as BookViewModel;
        if (bookVM == null) return;

        var book = await _db.GetBookById(bookVM.Id);
        if (book == null) return;

        await Navigation.PushAsync(new UpdateBookPage(_db, book));
    }

    private async void OnDeleteBookClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var bookVM = button?.CommandParameter as BookViewModel;
        if (bookVM == null) return;

        var book = await _db.GetBookById(bookVM.Id);
        if (book == null) return;

        bool confirmed = await DisplayAlert("Silme Onayı", $"{book.Title} kitabını silmek istediğinize emin misiniz?", "Evet", "Hayır");
        if (confirmed)
        {
            await _db.DeleteBook(book.Id);
            LoadBooks();
        }
    }

    private async void OnBorrowClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var bookVM = button?.CommandParameter as BookViewModel;

        if (bookVM == null || !bookVM.IsAvailable)
        {
            await DisplayAlert("Uyarı", "Bu kitap şu anda uygun değil.", "Tamam");
            return;
        }

        await _db.BorrowBook(App.CurrentUser.Id, bookVM.Id);
        await DisplayAlert("Başarılı", "Kitap ödünç alındı.", "Tamam");
        LoadBooks();
    }

    private async void OnReturnClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var bookVM = button?.CommandParameter as BookViewModel;

        if (bookVM == null) return;

        var borrowings = await _db.GetBorrowedBooksByUser(App.CurrentUser.Id);
        var borrowing = borrowings.FirstOrDefault(b => b.BookId == bookVM.Id);

        if (borrowing == null)
        {
            await DisplayAlert("Hata", "Bu kitap size ait bir ödünç alma kaydı bulunamadı.", "Tamam");
            return;
        }

        bool confirmed = await DisplayAlert("İade Onayı", $"{bookVM.Title} kitabını iade etmek istediğinize emin misiniz?", "Evet", "Hayır");
        if (confirmed)
        {
            await _db.ReturnBook(borrowing.Id);
            await DisplayAlert("Başarılı", "Kitap iade edildi.", "Tamam");
            LoadBooks();
        }
    }

    private async void OnBorrowedBooksClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BorrowedBooksPage(_db));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Çıkış", "Çıkmak istediğinize emin misiniz?", "Evet", "Hayır");
        if (confirm)
        {
            App.CurrentUser = null;
            await Navigation.PushAsync(new LoginPage(_db));
            Navigation.RemovePage(this);
        }
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePasswordPage(_db));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (App.CurrentUser == null)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage(_db));

        }
        else
        {
           
        WelcomeLabel.Text = $"👋 Hoş geldin, {App.CurrentUser.Username}";
        WelcomeLabel.Opacity = 0;
WelcomeLabel.FadeTo(1, 500);
    
            LoadBooks(); 
        }
    }

    private void OnFilterByYearClicked(object sender, EventArgs e)
    {
        if (int.TryParse(YearEntry.Text, out int year))
        {
            var filtered = _books.Where(b => b.PublishedYear == year).ToList();
            BooksCollectionView.ItemsSource = filtered;
        }
        else
        {
            DisplayAlert("Hata", "Lütfen geçerli bir yıl girin.", "Tamam");
        }
    }
    private void OnSearchClicked(object sender, EventArgs e)
{
    string query = SearchEntry.Text?.ToLower();
    if (!string.IsNullOrWhiteSpace(query))
    {
        var filtered = _books.Where(b => b.Title.ToLower().Contains(query)).ToList();
        BooksCollectionView.ItemsSource = filtered;
    }
    else
    {
        BooksCollectionView.ItemsSource = _books;
    }
}

private async void OnMyBooksClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new MyBooksPage(_db));
}
/*protected override void WelcomeMessage()
{
    base.OnAppearing();

    if (App.CurrentUser != null)
    {
        WelcomeLabel.Text = $"👋 Hoş geldin, {App.CurrentUser.Username}!";
    }

    // Diğer yüklemeler varsa burada kalabilir
}
*/

    private async void OnDeleteBookSwipe(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var book = swipeItem?.CommandParameter as BookViewModel;
        if (book == null) return;
        bool confirm = await DisplayAlert("Kitap Sil", $"{book.Title} kitabını silmek istediğinize emin misiniz?", "Evet", "Hayır");
        if (confirm)
        {
            await _db.DeleteBook(book.Id);
            LoadBooks();
        }
    }

}
