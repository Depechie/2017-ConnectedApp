using Xamarin.Forms;

namespace ConnectedAppNetStandard.Views
{
    public partial class PostOverviewPage : ContentPage
    {
        public PostOverviewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PostListView.SelectedItem = null;
        }
    }
}