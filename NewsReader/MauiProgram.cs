using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsReader.Services;
using NewsReader.ViewModels;
using NewsReader.Views;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;

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

            // ---------------------------------------------------------
            // 1. LOAD CONFIGURATION (appsettings.json) - embedded resource
            // ---------------------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            // Use the assembly namespace to avoid mismatches
            string resourceName = $"{typeof(MauiProgram).Namespace}.appsettings.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                // If this crashes, check that Build Action is set to "EmbeddedResource"
                throw new FileNotFoundException($"CRITICAL: Could not find '{resourceName}'. Did you set Build Action to 'Embedded Resource'?");
            }

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            // ---------------------------------------------------------
            // 2. REGISTER SETTINGS
            // ---------------------------------------------------------
            // Binds the "NewsApi" section from JSON to your C# class
            builder.Services.Configure<NewsApiOptions>(builder.Configuration.GetSection("NewsApi"));

            // Optional: validate BaseUrl early (additional safety)
            var baseUrl = builder.Configuration.GetSection("NewsApi")["BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl) || !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            {
                throw new Exception("ERROR: NewsApi:BaseUrl is missing or not a valid absolute URL. Check your appsettings.json.");
            }

            // ---------------------------------------------------------
            // 3. CONFIGURE HTTP CLIENT (typed client)
            // ---------------------------------------------------------
            builder.Services.AddHttpClient<INewsApiClient, NewsApiClient>((sp, http) =>
            {
                var options = sp.GetRequiredService<IOptions<NewsApiOptions>>().Value;

                // Safety check to ensure options loaded correctly
                if (string.IsNullOrWhiteSpace(options.BaseUrl) || !Uri.IsWellFormedUriString(options.BaseUrl, UriKind.Absolute))
                {
                    throw new Exception("ERROR: AppSettings loaded, but BaseUrl is empty or invalid. Check your JSON keys!");
                }

                http.BaseAddress = new Uri(options.BaseUrl);
                http.Timeout = TimeSpan.FromSeconds(30);

                // Properly add a User-Agent and Accept header
                http.DefaultRequestHeaders.UserAgent.Clear();
                http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("NewsReaderApp", "1.0"));
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // ---------------------------------------------------------
            // 4. REGISTER SERVICES & PAGES
            // ---------------------------------------------------------
            // ViewModels typically transient
            builder.Services.AddTransient<NewsListViewModel>();
            builder.Services.AddTransient<NewsDetailsViewModel>();

            // Pages can be transient (or singleton depending on navigation strategy)
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<NewsDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}