using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IDal _dal;
        public CategoriesController(IDal dal)
        {
            _dal = dal;
        }

        [Route("/categories/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            return View(await _dal.PostsToListAsync());
        }
    }
}