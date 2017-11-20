using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectedAppNetStandard.Models;
using ConnectedAppNetStandard.Services.Interfaces;
using Refit;

namespace ConnectedAppNetStandard.Services
{
    /// <summary>
    /// Actual API class
    /// Uses REFIT to retrieve backend data
    /// </summary>
    public class FakeAPI : IFakeAPI
    {
        public const string FakeAPIBase = "https://jsonplaceholder.typicode.com";

        private IFakeAPI _internalAPI;

        public FakeAPI()
        {
            _internalAPI = RestService.For<IFakeAPI>(FakeAPIBase);
        }

        public Task<List<Comment>> GetCommentsByPost(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPost(string id)
        {
            return _internalAPI.GetPost(id);
        }

        public Task<List<Post>> GetPosts()
        {
            return _internalAPI.GetPosts();
        }
    }
}