using BlogCore.Extensions;
using BlogCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogCore.Data
{
    public class MemoryCache : IDal
    {
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _postsFolder;
        private readonly string _categoriesFolder;
        private readonly Dictionary<string, Post> _posts = new Dictionary<string, Post>();
        private readonly Dictionary<string, Category> _categories = new Dictionary<string, Category>();
        private readonly Dictionary<string, List<string>> _tagPosts = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<string>> _catagoryPosts = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, string> _tags = new Dictionary<string, string>();


        public MemoryCache(IHostingEnvironment env, IHttpContextAccessor contextAccessor)
        {
            _env = env;
            _contextAccessor = contextAccessor;
            _categoriesFolder = Path.Combine(_env.WebRootPath, "Categories");
            _postsFolder = Path.Combine(_env.WebRootPath, "Posts");

            if (!Directory.Exists(_categoriesFolder)) Directory.CreateDirectory(_categoriesFolder);
            if (!Directory.Exists(_postsFolder)) Directory.CreateDirectory(_postsFolder);

            foreach (string file in Directory.EnumerateFiles(_categoriesFolder, "*.json", SearchOption.TopDirectoryOnly))
            {
                string json = File.ReadAllText(file, Encoding.Default);
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(json);
                foreach (Category c in categories)
                {
                    _categories.Add(c.Id, c);
                }
            }

            foreach (string file in Directory.EnumerateFiles(_postsFolder, "*.json", SearchOption.TopDirectoryOnly))
            {
                string json = File.ReadAllText(file, Encoding.Default);
                Post post = JsonConvert.DeserializeObject<Post>(json);

                if (_categories.TryGetValue(post.CategoryId, out Category _cat))
                {
                    post.Category = _cat;
                    if (_catagoryPosts.ContainsKey(post.CategoryId))
                    {
                        _catagoryPosts[post.CategoryId].Add(post.Id);
                    }
                    else
                    {
                        _catagoryPosts.Add(post.CategoryId, new List<string> { post.Id });
                    }
                }
                _posts.Add(post.Id, post);

                if (post.Tags != null && post.Tags.Count > 0)
                {
                    foreach (string tag in post.Tags)
                    {
                        string _tag = tag.Conform();
                        if (_tagPosts.TryGetValue(_tag, out List<string> _postSlugs))
                        {
                            _postSlugs.Add(post.Slug);
                        }
                        else
                        {
                            _tagPosts.Add(_tag, new List<string> { post.Slug });
                        }

                        if (!_tags.ContainsKey(_tag))
                        {
                            _tags.Add(_tag, tag);
                        }
                    }
                }
            }
        }

        public async Task<List<Category>> CategoriesToListAsync()
        {
            List<Category> list = await Task.Run(() =>
            {
                return _categories.Values.ToList();
            });
            return list;
        }

        public async Task<List<Post>> PostsToListAsync(bool onlyVisible = true)
        {
            List<Post> list = await Task.Run(() =>
            {
                if (onlyVisible)
                {
                    return _posts.Values.Where(p => p.IsPublished == true).OrderByDescending(o => o.Order).ToList();
                }
                else
                {
                    return _posts.Values.OrderByDescending(o => o.Order).ToList();
                }
            });
            return list;
        }

        public async Task<List<Post>> PostsToListAsync(int take, int skip = 0, bool onlyVisible = true)
        {
            List<Post> list = await Task.Run(() =>
            {
                if (onlyVisible)
                {
                    return _posts.Values.Where(p => p.IsPublished == true).OrderByDescending(o => o.Order).Take(take).Skip(skip).ToList();
                }
                else
                {
                    return _posts.Values.OrderByDescending(o => o.Order).Take(take).Skip(skip).ToList();
                }
            });
            return list;
        }

        public async Task<List<Post>> PostsToListAsync(string tag, int take, int skip = 0, bool onlyVisible = true)
        {
            if (_tagPosts.TryGetValue(tag, out List<string> _slugs))
            {
                List<Post> list = await Task.Run(() =>
                {
                    if (onlyVisible)
                    {
                        return _posts.Where(p => _slugs.Contains(p.Value.Slug) && p.Value.IsPublished == true).Select(s => s.Value).Take(take).Skip(skip).ToList();
                    }
                    else
                    {
                        return _posts.Where(p => _slugs.Contains(p.Value.Slug)).Select(s => s.Value).Take(take).Skip(skip).ToList();
                    }
                });
                return list;
            }
            return null;
        }

        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            Post post = await Task.Run(() =>
            {
                return _posts.Values.Where(p => p.Slug == slug).FirstOrDefault();
            });
            return post;
        }

        public async Task<string> TagName(string tag)
        {
            return await Task.Run(() =>
            {
                if (_tags.TryGetValue(tag, out string _tag))
                {
                    return _tag;
                }
                else
                {
                    return "Unknown tag";
                }
            });
        }

        public async Task Add(Post p)
        {
            await Task.Run(() =>
            {
                _posts.Add(p.Id, p);
                string serial = JsonConvert.SerializeObject(p);

                FileStream stream = File.Create(_postsFolder + "/" + p.Id + ".json");
                byte[] bytes = Encoding.Default.GetBytes(serial);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                stream.Dispose();
            });
        }

        public async Task UpdateAsync(Post post)
        {
            await Task.Run(() =>
            {
                if (_posts.ContainsKey(post.Id))
                {
                    _posts[post.Id] = post;

                    string serial = JsonConvert.SerializeObject(post);
                    FileStream stream = File.Open(_postsFolder + "/" + post.Id + ".json", FileMode.Open);
                    int streamLength = Convert.ToInt32(stream.Length);
                    byte[] bytes = Encoding.Default.GetBytes(serial);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.SetLength(bytes.Length);

                    stream.Close();
                    stream.Dispose();
                }
            });
        }

        public Category CategoryById(string id)
        {
            if (_categories.TryGetValue(id, out Category _cat))
            {
                return _cat;
            }
            else
            {
                return null;
            }
        }

        public int GetOrderNumber()
        {
            int? _order = _posts.Values.OrderByDescending(o => o.Order).FirstOrDefault()?.Order;
            if (_order.HasValue)
            {
                return _order.Value + 1;
            }
            else
            {
                return 1;
            }
        }

        public List<string> Tags(string text)
        {
            return _tags.Values.Where(t => t.ToLower().Contains(text.ToLower())).ToList();
        }

        public async Task<Post> LastPostAsync()
        {
            return await Task.Run(() =>
            {
                return _posts.Values.Where(p => p.IsPublished == true).OrderByDescending(o => o.Order).FirstOrDefault();
            });
        }

        public async Task<ManageViewModel> ManageViewModelAsync()
        {
            return await Task.Run(() =>
            {
                return new ManageViewModel
                {
                    Posts = _posts.Values
                      .OrderByDescending(o => o.Order)
                      .Select(s => new Dictionary<string, string>
                      {
                        { "Id",s.Id},
                        { "Title",s.Title},
                        {"PublishDate",s.PublishDate.ToShortDateString() },
                        {"IsPublished",s.IsPublished.ToString() }
                      })
                      .ToList()
                };
            });
        }

        public async Task<Post> GetPostByIdAsync(string id)
        {
            return await Task.Run(() =>
            {
                if (_posts.TryGetValue(id, out Post post))
                {
                    return post;
                }
                else
                {
                    return null;
                }
            });
        }

        public int PostCount(string id)
        {
            if (_catagoryPosts.ContainsKey(id))
            {
                return _catagoryPosts[id].Count;
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<Post>> PostsToListAsync(string categoryId)
        {
            return await Task.Run(() =>
            {
                return _posts.Values.Where(p => p.CategoryId == categoryId && p.IsPublished == true).OrderByDescending(o => o.Order).ToList();
            });
        }
    }
}
