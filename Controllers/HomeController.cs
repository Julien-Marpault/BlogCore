using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogCore.Models;
using BlogCore.Data;

namespace BlogCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDal _dal;
        public HomeController(IDal dal)
        {
            _dal = dal;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dal.LastPostAsync());
        }

        [Route("/categories")]
        public async Task<IActionResult> Categories()
        {
            return View(await _dal.CategoriesToListAsync());
        }

        [Route("/posts")]
        public async Task<IActionResult> Posts()
        {
            return View(await _dal.PostsToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("/error/{code}")]
        public IActionResult Error(int code)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
