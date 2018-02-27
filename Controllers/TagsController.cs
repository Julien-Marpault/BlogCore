using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogCore.Controllers
{
    public class TagsController : Controller
    {
        private readonly IDal _dal;

        public TagsController(IDal dal)
        {
            _dal = dal;
        }

        
    }
}