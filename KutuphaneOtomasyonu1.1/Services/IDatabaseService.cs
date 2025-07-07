using System;
using System.Collections.Generic;
using KutuphaneOtomasyonu.Models;
using System.Threading.Tasks;

namespace KutuphaneOtomasyonu.Services
{
    public interface IDatabaseService
    {
        Task<List<Book>> GetBooksByMember(int memberId);
        Task<List<Book>> GetBooks();
        Task<int> AddUser(User user);
        Task<User> GetUserByUsername(string username);
        Task<bool> ValidateUser(string username, string password);
        User GetUserByUsernameAndPassword(string username, string password);
        Task<bool> DeleteBook(int bookId);
        Task<int> BorrowBook(int userId, int bookId);
        Task<List<Borrowing>> GetBorrowingsByUser(int userId);
        Task<List<Borrowing>> GetBorrowedBooksByUser(int userId);
        Task<bool> ReturnBook(int borrowingId);
        Task<Book> GetBookById(int bookId);
        Task<List<BookViewModel>> GetAllBooksWithBorrowInfo(int currentUserId);
        int UpdateUserPasswordWithResult(int userId, string newPassword);
        Task<bool> UpdateUserPassword(int userId, string oldPassword, string newPassword);
        int AddMember(Member member);
        Task<int> AddBook(Book book);
        Task<bool> UpdateBook(Book book);
    }
} 