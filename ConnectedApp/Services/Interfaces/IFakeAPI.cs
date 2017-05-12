using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectedApp.Models;
using Refit;

namespace ConnectedApp.Services.Interfaces
{
    [Headers("Accept: application/json")]
    public interface IFakeAPI
    {
        [Get("/posts")]
        Task<List<Post>> GetPosts();

        [Get("/posts/{id}")]
        Task<Post> GetPost(string id);

        [Get("/posts/{id}/comments")]
        Task<List<Comment>> GetCommentsByPost(string id);
    }
}