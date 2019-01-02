using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglynionBedd.Models;
using Microsoft.AspNetCore.Http;

namespace EnglynionBedd.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LlwythoDelwedd(IFormFile file)
        {
            var gwybodaeth = new GwybodaethDelwedd();

            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot",
                file.FileName);

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                gwybodaeth.Delwedd = stream.ToArray();
            }

            return View(gwybodaeth);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
