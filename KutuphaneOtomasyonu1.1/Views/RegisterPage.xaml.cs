using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;

namespace KutuphaneOtomasyonu.Views;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _db;

    public RegisterPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text?.Trim();
        string password = passwordEntry.Text;
        string confirmPassword = confirmPasswordEntry.Text;

        string gender = femaleRadio.IsChecked ? "Kadın"
                      : maleRadio.IsChecked ? "Erkek"
                      : noneRadio.IsChecked ? "Belirtmek istemiyorum"
                      : null;

        string userType = userTypePicker.SelectedItem?.ToString();
        DateTime birthDate = birthDatePicker.Date;

        bool acceptedTerms = termsCheckBox.IsChecked;
        if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 20)
        {
            await DisplayAlert("Hata", "Kullanıcı adı 3-20 karakter arası olmalıdır.", "Tamam");
            return;
        }
        if (string.IsNullOrEmpty(password) || password.Length < 6 || password.Length > 20)
        {
            await DisplayAlert("Hata", "Şifre 6-20 karakter arası olmalıdır.", "Tamam");
            return;
        }
        if (password != confirmPassword)
        {
            await DisplayAlert("Hata", "Şifreler eşleşmiyor", "Tamam");
            return;
        }
        if (!acceptedTerms)
        {
            await DisplayAlert("Hata", "Kullanım şartlarını kabul etmelisiniz.", "Tamam");
            return;
        }
        if (string.IsNullOrEmpty(userType))
        {
            await DisplayAlert("Hata", "Lütfen bir kullanıcı türü seçiniz.", "Tamam");
            return;
        }
        if (birthDate > DateTime.Now.AddYears(-10) || birthDate < DateTime.Now.AddYears(-120))
        {
            await DisplayAlert("Hata", "Geçerli bir doğum tarihi giriniz.", "Tamam");
            return;
        }
        if (string.IsNullOrEmpty(gender))
        {
            await DisplayAlert("Hata", "Cinsiyet seçiniz.", "Tamam");
            return;
        }
        var existingUser = await _db.GetUserByUsername(username);
        if (existingUser != null)
        {
            await DisplayAlert("Hata", "Bu kullanıcı adı zaten kayıtlı", "Tamam");
            return;
        }

        var user = new User
        {
            Username = username,
            Password = password,
            Gender = gender,
            UserType = userType,
            BirthDate = birthDate,
            CreatedAt = DateTime.Now
        };

        var result = await _db.AddUser(user);
        if (result > 0)
        {
            await DisplayAlert("Başarılı", "Kayıt tamamlandı, giriş yapabilirsiniz", "Tamam");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Hata", "Kayıt başarısız oldu. Lütfen tekrar deneyin.", "Tamam");
        }
    }
}
