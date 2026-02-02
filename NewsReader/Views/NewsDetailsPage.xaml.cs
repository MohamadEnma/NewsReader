using NewsReader.ViewModels;
namespace NewsReader.Views;

public partial class NewsDetailsPage : ContentPage
{
    public NewsDetailsPage(NewsDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}