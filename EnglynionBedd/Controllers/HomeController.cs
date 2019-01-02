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
        public async Task<IActionResult> LlwythoDelwedd(IFormFile ffeil)
        {
            var gwybodaeth = new GwybodaethDelwedd();

            if (ffeil == null || ffeil.Length == 0)
                return Content("heb ddewis ffeil");

            using (var ffrwd = new MemoryStream())
            {
                await ffeil.CopyToAsync(ffrwd);
                gwybodaeth.Delwedd = ffrwd.ToArray();
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
