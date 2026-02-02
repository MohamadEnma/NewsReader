using NewsReader.ViewModels;

namespace NewsReader.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly NewsListViewModel _viewModel;

        public MainPage(NewsListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        } 
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.LoadCommand.CanExecute(null))
            {
                await _viewModel.LoadCommand.ExecuteAsync(null);
            }
        }
    }
}