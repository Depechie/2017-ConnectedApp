using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ConnectedApp.Views
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
