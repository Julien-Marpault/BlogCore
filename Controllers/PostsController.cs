using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Services;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace BlogCore.Controllers
{
    public class PostsController : Controller
    {
        private readonly IDal _dal;

        public PostsController(IDal dal)
        {
            _dal = dal;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _dal.PostsToListAsync(10, 0));
        }

        // GET: slug-of-the-post
        [Route("/posts/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            Post post = await _dal.GetPostBySlugAsync(slug);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize]
        [HttpGet("/post/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            Post p = await _dal.GetPostByIdAsync(id);

            EditPostViewModel vm = new EditPostViewModel
            {
                PublishDate = p.PublishDate,
                RevisionDate = p.RevisionDate,
                CategoryId = p.CategoryId,
                Content = p.Content,
                Tags = p.Tags,
                Title = p.Title,
                Categories = new SelectList(await _dal.CategoriesToListAsync(), "Id", "Name")
            };
            return View(vm);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("/post/edit/{id}")]
        public async Task<IActionResult> Edit(string id, EditPostViewModel vm)
        {
            Post p = await _dal.GetPostByIdAsync(vm.Id);

            foreach (PropertyInfo property in typeof(EditPostViewModel).GetProperties())
            {
                PropertyInfo _property = typeof(Post).GetProperty(property.Name);
                if (_property != null)
                {
                    _property.SetValue(p, property.GetValue(vm));
                }
            }
            //TODO: Handle tag and category modification for Dal
            await _dal.UpdateAsync(p);
            return RedirectToAction("Index", "Manage");
        }

        //[Authorize]
        //[ValidateAntiForgeryToken]
        [HttpPost("/post/order")]
        public async Task<IActionResult> Order(IFormCollection collection)
        {
            collection.TryGetValue("id", out StringValues id);
            collection.TryGetValue("insert", out StringValues insert);
            collection.TryGetValue("target", out StringValues target);

            Post p = await _dal.GetPostByIdAsync(id);
            Post t = await _dal.GetPostByIdAsync(target);
            if (p == null || t == null) return new NotFoundResult();
            if (insert == "before")
            {
                int targetOrder = t.Order;
                p.Order = targetOrder + 1;
                foreach (Post _p in await _dal.PostsToListAsync(false))
                {
                    if (_p.Order >= p.Order)
                    {
                        _p.Order = _p.Order + 1;
                        await _dal.UpdateAsync(_p);
                    }
                }
                await _dal.UpdateAsync(p);
                return Content("0");
            }
            else if (insert == "after")
            {
                int targetOrder = t.Order;
                p.Order = targetOrder;
                t.Order = t.Order + 1;
                foreach (Post _p in await _dal.PostsToListAsync(false))
                {
                    if (_p.Order >= t.Order)
                    {
                        _p.Order = _p.Order + 1;
                        await _dal.UpdateAsync(_p);
                    }
                }
                await _dal.UpdateAsync(p);
                await _dal.UpdateAsync(t);
                return Content("0");
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [Authorize]
        [HttpGet("/post/toggleonline/{id}")]
        public async Task<IActionResult> ToggleOnline(string id)
        {
            Post p = await _dal.GetPostByIdAsync(id);
            if (p.IsPublished == false)
            {
                p.IsPublished = true;
            }
            else
            {
                p.IsPublished = false;
            }
            await _dal.UpdateAsync(p);
            return RedirectToAction("Index", "Manage");
        }

        [Route("/tags/{tag}")]
        public async Task<IActionResult> Tag(string tag)
        {
            ViewData["tag"] = await _dal.TagName(tag);
            List<Post> _posts = await _dal.PostsToListAsync(tag, 10, 0);
            return View(_posts);
        }

        [HttpGet("/completetags/{text}")]
        public string Index(string text)
        {
            List<string> _tags = _dal.Tags(text);
            return JsonConvert.SerializeObject(_tags);
        }
    }
}
