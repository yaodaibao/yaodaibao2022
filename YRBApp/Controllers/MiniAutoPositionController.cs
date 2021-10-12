using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YRBApp.Controllers
{
    public class MiniAutoPositionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> SaveLocation()
        {
            return "";
        }
    }
}
