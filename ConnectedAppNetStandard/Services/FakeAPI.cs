using System;
using System.Collections.Generic;
using System.Net;
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

        public int Retry { get; set; }

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

        public Task<List<Post>> GetPostsFallback()
        {
            return _internalAPI.GetPosts();
        }

        public Task<List<Post>> GetPosts()
        {
            return ExceptionWrapper();
        }

        private Task<List<Post>> ExceptionWrapper()
        {
            ++Retry;
            System.Diagnostics.Debug.WriteLine($"Service retry {Retry}");

            if (Retry >= 3)
            {
                System.Diagnostics.Debug.WriteLine("Service working");
                return _internalAPI.GetPosts();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Service exception");
                throw new WebException("Return dummy exception for debugging purpose");
            }
        }
    }
}