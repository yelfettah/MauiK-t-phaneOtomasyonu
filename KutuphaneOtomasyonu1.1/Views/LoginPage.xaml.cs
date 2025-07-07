using KutuphaneOtomasyonu.Services;

namespace KutuphaneOtomasyonu.Views;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;

    public LoginPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text?.Trim();
        string password = passwordEntry.Text;

      
        if (string.IsNullOrEmpty(username))
        {
            await DisplayAlert("Hata", "Kullanıcı adı boş bırakılamaz.", "Tamam");
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "Şifre boş bırakılamaz.", "Tamam");
            return;
        }

        if (password.Length < 1)
        {
            await DisplayAlert("Hata", "Şifre en az 1 karakter olmalıdır.", "Tamam");
            return;
        }

        if (await _db.ValidateUser(username, password))
        {
            App.CurrentUser = await _db.GetUserByUsername(username);
            App.CurrentUserr = _db.GetMemberByUsername(username);

            await DisplayAlert("Giriş Başarılı", $"Hoş geldiniz, {App.CurrentUser.Username}!", "Tamam");

            try
            {
                await Navigation.PushAsync(new BookListPage(_db));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigasyon Hatası", ex.Message, "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hatalı Giriş", "Kullanıcı adı veya şifre yanlış", "Tamam");
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(_db));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        usernameEntry.Opacity = 0;
        passwordEntry.Opacity = 0;
        loginButton.Opacity = 0;
        registerButton.Opacity = 0;

        usernameEntry.TranslationY = -30;
        passwordEntry.TranslationY = -30;
        loginButton.TranslationY = -30;
        registerButton.TranslationY = -30;

        await usernameEntry.FadeTo(1, 400);
        await usernameEntry.TranslateTo(0, 0, 400, Easing.CubicOut);

        await passwordEntry.FadeTo(1, 400);
        await passwordEntry.TranslateTo(0, 0, 400, Easing.CubicOut);

        await loginButton.FadeTo(1, 400);
        await loginButton.TranslateTo(0, 0, 400, Easing.CubicOut);

        await registerButton.FadeTo(1, 400);
        await registerButton.TranslateTo(0, 0, 400, Easing.CubicOut);
    }
}
