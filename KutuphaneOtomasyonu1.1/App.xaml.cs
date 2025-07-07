using KutuphaneOtomasyonu.Services;
using KutuphaneOtomasyonu.Views;
using KutuphaneOtomasyonu.Models;


namespace KutuphaneOtomasyonu;

public partial class App : Application
{
    public static DatabaseService DatabaseServiceInstance { get; private set; }
	public static User CurrentUser { get; set; }
	public static Member CurrentUserr { get; set; }



    public App(DatabaseService dbService)
    {
        InitializeComponent();

        DatabaseServiceInstance = dbService;
		MainPage = new NavigationPage(new LoginPage(DatabaseServiceInstance));
    

       //MainPage = new NavigationPage(new LoginPage());
    }
}
