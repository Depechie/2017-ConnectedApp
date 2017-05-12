using System;
using System.Collections.ObjectModel;
using System.Linq;
using ConnectedApp.Extensions;
using ConnectedApp.Models;
using ConnectedApp.Services.Interfaces;
using Prism.Navigation;

namespace ConnectedApp.ViewModels
{
    public class PostOverviewViewModel : ViewModelBase
    {
        private readonly IFakeService _fakeService;

        private ObservableCollection<Post> _posts = new ObservableCollection<Post>();
        public ObservableCollection<Post> Posts
        {
            get { return _posts; }
            set { SetProperty(ref _posts, value); }
        }

        private Post _selectedPost;
        public Post SelectedPost
        {
            get { return _selectedPost; }
            set
            {
                SetProperty(ref _selectedPost, value);
                if (value != null)
                    _navigationService.NavigateAsync($"PostDetailPage?id={_selectedPost.Id}");
            }
        }

        public IFakeService FakeService
        {
            get
            {
                return _fakeService;
            }
        }

        public PostOverviewViewModel(INavigationService navigationService, IFakeService fakeService) : base(navigationService)
        {
            _fakeService = fakeService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            //TODO: Validate if loading posts each time we navigate to the page is a good idea? We will also trigger this going back and forth to detail
            _fakeService.GetPosts().Subscribe(items =>
            {
                //Only clear and merge posts if we retrieve actual data
                if (items != null && items.Any())
                {
                    //TODO: Create a correct merge operation
                    Posts.Clear();
                    Posts.Merge(items);
                }
            });
        }
    }
}