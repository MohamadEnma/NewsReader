using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReader.Models;
using NewsReader.Services;
using System.Collections.ObjectModel;

namespace NewsReader.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly NewsApiClient newsService;
        [ObservableProperty]
        ObservableCollection<ArticleDto> articles = new ObservableCollection<ArticleDto>();

        public MainViewModel(NewsApiClient newsService)
        {
            this.newsService = newsService;
        }

     
    }
}
