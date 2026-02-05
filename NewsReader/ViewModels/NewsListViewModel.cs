using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReader.Models;
using NewsReader.Services;
using System.Collections.ObjectModel;

namespace NewsReader.ViewModels
{
    public partial class NewsListViewModel : ObservableObject
    {
        private readonly INewsApiClient _client;

        public ObservableCollection<ArticleDto> Articles { get; } = new();

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string? errorMessage;

        [ObservableProperty] private string searchText;
        [ObservableProperty] private string? selectedCategory;

        public IReadOnlyList<string> Categories { get; } =
          new[] { "business", "entertainment", "general", "health", "science", "sports", "technology" };

        public NewsListViewModel(INewsApiClient client)
        {
            _client = client;
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            ErrorMessage = null;
            IsBusy = true;

            try
            {
                var q = new NewsQuery
                {
                    Country = "se",
                    SearchText = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                    PageSize = 20,
                    Category = string.IsNullOrWhiteSpace(selectedCategory) ? "general" : selectedCategory
                };

                var items = await _client.GetTopHeadlinesAsync(q);
                Articles.Clear();
                foreach (var a in items)
                    Articles.Add(a);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
            
        }


        [RelayCommand]
        public async Task InitializeAsync()
        {
            await LoadAsync();
        }



        [RelayCommand]
        private async Task OpenDetails(ArticleDto? article)
        {
            if (article is null) return;

            
            var navigationParameter = new Dictionary<string, object>
    {
        { "Article", article }
    };

            await Shell.Current.GoToAsync("NewsDetailsPage", navigationParameter);
        }
    }
}
