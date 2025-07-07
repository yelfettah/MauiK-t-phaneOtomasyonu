using Microsoft.Extensions.Logging;
using KutuphaneOtomasyonu.Services;
using KutuphaneOtomasyonu.Views;

namespace KutuphaneOtomasyonu;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "library.db");

      
        builder.Services.AddSingleton(new DatabaseService(dbPath));
        
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<BookListPage>();
        builder.Services.AddSingleton<AddBookPage>();
        builder.Services.AddTransient<RegisterPage>();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
