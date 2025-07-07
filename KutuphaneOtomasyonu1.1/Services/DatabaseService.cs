using SQLite;
using KutuphaneOtomasyonu.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace KutuphaneOtomasyonu.Services;

public class DatabaseService : IDatabaseService
{
    private readonly SQLiteConnection _database;
    private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5107") };

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteConnection(dbPath);
        _database.CreateTable<Book>();
        _database.CreateTable<Member>();
        _database.CreateTable<Borrowing>();
        _database.CreateTable<User>();  
    }

    public async Task<List<Book>> GetBooksByMember(int memberId)
    {
        var response = await _httpClient.GetAsync("/api/books");
        if (response.IsSuccessStatusCode)
        {
            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            return books?.Where(b => b.MemberId == memberId).ToList() ?? new List<Book>();
        }
        return new List<Book>();
    }

    public async Task<List<Book>> GetBooks()
    {
        var response = await _httpClient.GetAsync("/api/books");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<Book>>();
        return new List<Book>();
    }

    public async Task<int> AddUser(User user)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/users", user);
        if (response.IsSuccessStatusCode)
        {
            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            return createdUser?.Id ?? 0;
        }
        return 0;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        var response = await _httpClient.GetAsync($"/api/users");
        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            return users?.FirstOrDefault(u => u.Username == username);
        }
        return null;
    }

    public async Task<bool> ValidateUser(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/users/login", new { Username = username, Password = password });
        return response.IsSuccessStatusCode;
    }

    public User GetUserByUsernameAndPassword(string username, string password)
    {
        return _database.Table<User>()
            .FirstOrDefault(u => u.Username == username && u.Password == password);
    }

    public async Task<bool> DeleteBook(int bookId)
    {
        var response = await _httpClient.DeleteAsync($"/api/books/{bookId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<int> BorrowBook(int userId, int bookId)
    {
        var borrowing = new Borrowing { UserId = userId, BookId = bookId };
        var response = await _httpClient.PostAsJsonAsync("/api/borrowings", borrowing);
        if (response.IsSuccessStatusCode)
        {
            var created = await response.Content.ReadFromJsonAsync<Borrowing>();
            return created?.Id ?? 0;
        }
        return 0;
    }

    public async Task<List<Borrowing>> GetBorrowingsByUser(int userId)
    {
        var response = await _httpClient.GetAsync("/api/borrowings");
        if (response.IsSuccessStatusCode)
        {
            var allBorrowings = await response.Content.ReadFromJsonAsync<List<Borrowing>>();
            return allBorrowings?.Where(b => b.UserId == userId).ToList() ?? new List<Borrowing>();
        }
        return new List<Borrowing>();
    }

    public async Task<List<Borrowing>> GetBorrowedBooksByUser(int userId)
    {
        var response = await _httpClient.GetAsync("/api/borrowings");
        if (response.IsSuccessStatusCode)
        {
            var allBorrowings = await response.Content.ReadFromJsonAsync<List<Borrowing>>();
            return allBorrowings?.Where(b => b.UserId == userId && b.ReturnDate == null).ToList() ?? new List<Borrowing>();
        }
        return new List<Borrowing>();
    }

    public async Task<bool> ReturnBook(int borrowingId)
    {
        var response = await _httpClient.PutAsync($"/api/borrowings/{borrowingId}/return", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<Book> GetBookById(int bookId)
    {
        var response = await _httpClient.GetAsync($"/api/books/{bookId}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<Book>();
        return null;
    }

    public async Task<List<BookViewModel>> GetAllBooksWithBorrowInfo(int currentUserId)
    {
        var booksResponse = await _httpClient.GetAsync("/api/books");
        var borrowingsResponse = await _httpClient.GetAsync("/api/borrowings");
        var usersResponse = await _httpClient.GetAsync("/api/users");
        if (booksResponse.IsSuccessStatusCode && borrowingsResponse.IsSuccessStatusCode && usersResponse.IsSuccessStatusCode)
        {
            var books = await booksResponse.Content.ReadFromJsonAsync<List<Book>>();
            var borrowings = await borrowingsResponse.Content.ReadFromJsonAsync<List<Borrowing>>();
            var users = await usersResponse.Content.ReadFromJsonAsync<List<User>>();
            var bookViewModels = books.Select(book =>
            {
                var borrowing = borrowings.FirstOrDefault(b => b.BookId == book.Id && b.ReturnDate == null);
                string borrowedByUserName = null;
                bool isBorrowedByCurrentUser = false;
                if (borrowing != null)
                {
                    var user = users.FirstOrDefault(u => u.Id == borrowing.UserId);
                    borrowedByUserName = user?.Username;
                    isBorrowedByCurrentUser = borrowing.UserId == currentUserId;
                }
                var addedByUser = users.FirstOrDefault(u => u.Id == book.MemberId);
                string addedByUserName = addedByUser?.Username ?? "Bilinmiyor";
                return new BookViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    IsAvailable = book.IsAvailable,
                    MemberId = book.MemberId,
                    BorrowedByUserName = borrowedByUserName,
                    IsBorrowedByCurrentUser = isBorrowedByCurrentUser,
                    AddedByUserName = addedByUserName
                };
            }).ToList();
            return bookViewModels;
        }
        return new List<BookViewModel>();
    }

    public int UpdateUserPasswordWithResult(int userId, string newPassword)
    {
        var user = _database.Find<User>(userId);
        if (user == null) return 0;

        user.Password = newPassword;
        return _database.Update(user);
    }

    public async Task<bool> UpdateUserPassword(int userId, string oldPassword, string newPassword)
    {
        var req = new { OldPassword = oldPassword, NewPassword = newPassword };
        var response = await _httpClient.PutAsJsonAsync($"/api/users/{userId}/changepassword", req);
        return response.IsSuccessStatusCode;
    }

    public Member GetMemberByUsername(string username)
    {
        return _database.Table<Member>().FirstOrDefault(m => m.Name == username);
    }

    public int AddMember(Member member)
    {
        return _database.Insert(member);
    }

    public async Task<int> AddBook(Book book)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/books", book);
        if (response.IsSuccessStatusCode)
        {
            var createdBook = await response.Content.ReadFromJsonAsync<Book>();
            return createdBook?.Id ?? 0;
        }
        return 0;
    }

    public async Task<bool> UpdateBook(Book book)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/books/{book.Id}", book);
        return response.IsSuccessStatusCode;
    }
}
