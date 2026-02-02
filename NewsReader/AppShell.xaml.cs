namespace NewsReader
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("NewsDetailsPage", typeof(Views.NewsDetailsPage));
        }
    }
}
