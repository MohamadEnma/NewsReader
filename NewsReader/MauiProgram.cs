using Microsoft.Extensions.Logging;
using NewsReader.Services;
using NewsReader.ViewModels;
using NewsReader.Views;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;

namespace NewsReader
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



            // Registrera inställningar 
            builder.Services.Configure<NewsApiOptions>(options =>
            {
                options.ApiKey = "fbdf44c2b83b45a1a581db4c6ce78f73";
                options.BaseUrl = "https://newsapi.org";
            });

            // HttpClient och Service 
            builder.Services.AddHttpClient<INewsApiClient, NewsApiClient>();

            // ViewModels och Pages
            builder.Services.AddTransient<NewsListViewModel>();
            builder.Services.AddTransient<NewsDetailsViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<NewsDetailsPage>();

            return builder.Build();
        }
    }
}
