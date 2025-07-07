using KutuphaneOtomasyonu.Models;
using KutuphaneOtomasyonu.Services;

namespace KutuphaneOtomasyonu.Views;

public partial class ChangePasswordPage : ContentPage
{
    private readonly DatabaseService _db;

    public ChangePasswordPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

   private async void OnChangePasswordClicked(object sender, EventArgs e)
{
    if (App.CurrentUser == null)
    {
        await DisplayAlert("Hata", "Kullanıcı bilgisi bulunamadı.", "Tamam");
        return;
    }

    var currentPassword = CurrentPasswordEntry.Text?.Trim();
    var newPassword = NewPasswordEntry.Text?.Trim();
    var confirmPassword = ConfirmPasswordEntry.Text?.Trim();

    if (string.IsNullOrWhiteSpace(currentPassword) || 
        string.IsNullOrWhiteSpace(newPassword) || 
        string.IsNullOrWhiteSpace(confirmPassword))
    {
        await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
        return;
    }

    if (App.CurrentUser.Password != currentPassword)
    {
        await DisplayAlert("Hata", "Mevcut şifreniz hatalı.", "Tamam");
        return;
    }

    if (newPassword != confirmPassword)
    {
        await DisplayAlert("Hata", "Yeni şifreler eşleşmiyor.", "Tamam");
        return;
    }

    try
    {
        bool result = await _db.UpdateUserPassword(App.CurrentUser.Id, currentPassword, newPassword);

        if (result)
        {
            App.CurrentUser.Password = newPassword; 
            await DisplayAlert("Başarılı", "Şifre başarıyla güncellendi.", "Tamam");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Hata", "Şifre güncellenemedi. Mevcut şifre yanlış olabilir.", "Tamam");
        }
    }
    catch (Exception ex)
    {
        await DisplayAlert("Hata", $"Beklenmedik hata oluştu: {ex.Message}", "Tamam");
    }
}

}
