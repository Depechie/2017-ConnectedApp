using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ConnectedApp.Models;
using ConnectedApp.Services.Interfaces;
using Refit;

namespace ConnectedApp.Services
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

        public Task<List<Post>> GetPosts()
        {
            return ExceptionWrapper();
        }

        private Task<List<Post>> ExceptionWrapper()
        {
            ++Retry;
            if (Retry == 3)
                return _internalAPI.GetPosts();
            else
                throw new WebException("Return dummy exception for debugging purpose");
        }
    }
}