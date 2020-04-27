using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class FilterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}