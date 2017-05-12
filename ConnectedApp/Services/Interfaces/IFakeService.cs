using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectedApp.Models;

namespace ConnectedApp.Services.Interfaces
{
    public interface IFakeService
    {
        Task<List<Post>> GetPosts();
        Task<Post> GetPost(string id);
        Task<List<Comment>> GetCommentsByPost(string id);
    }
}