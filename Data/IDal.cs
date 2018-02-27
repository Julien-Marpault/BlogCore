using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Data
{
    public interface IDal
    {
        Task<List<Category>> CategoriesToListAsync();
        Task<List<Post>> PostsToListAsync(bool onlyVisible = true);
        Task<List<Post>> PostsToListAsync(string categoryId);
        Task<List<Post>> PostsToListAsync(int take, int skip, bool onlyVisible = true);
        Task<List<Post>> PostsToListAsync(string tag, int take, int skip, bool onlyVisible = true);
        Task<Post> GetPostBySlugAsync(string slug);
        Task<string> TagName(string tag);
        Task Add(Post post);
        Category CategoryById(string id);
        int GetOrderNumber();
        List<string> Tags(string text);
        Task<Post> LastPostAsync();
        Task<ManageViewModel> ManageViewModelAsync();
        Task<Post> GetPostByIdAsync(string id);
        Task UpdateAsync(Post post);
        int PostCount(string id);
    }
}
