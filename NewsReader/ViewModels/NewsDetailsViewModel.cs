using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReader.Models;

namespace NewsReader.ViewModels;

[QueryProperty(nameof(Article), "Article")]
public partial class NewsDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    private ArticleDto? article;

    [RelayCommand]
    private async Task OpenBrowserAsync()
    {
        if (Article?.Url != null)
        {
            await Launcher.Default.OpenAsync(Article.Url);
        }
    }
}