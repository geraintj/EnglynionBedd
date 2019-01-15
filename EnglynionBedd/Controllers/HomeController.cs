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
using EnglynionBedd.Endidau;
using EnglynionBedd.Gwasanaethau;
using Microsoft.Extensions.Options;

namespace EnglynionBedd.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGwasanaethauGwybodol _gwasanaethauGwybodol;
        private readonly ICronfaBeddargraff _cronfaBeddargraff;

        public HomeController(IGwasanaethauGwybodol gwasanaethauGwybodol, ICronfaBeddargraff cronfaBeddargraff)
        {
            _gwasanaethauGwybodol = gwasanaethauGwybodol;
            _cronfaBeddargraff = cronfaBeddargraff;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LlwythoDelwedd(IFormFile ffeil)
        {
            GwybodaethDelwedd gwybodaeth = new GwybodaethDelwedd();

            if (ffeil == null || ffeil.Length == 0)
                return Content("heb ddewis ffeil");

            using (var ffrwd = new MemoryStream())
            {
                await ffeil.CopyToAsync(ffrwd);
                gwybodaeth = await _gwasanaethauGwybodol.DadansoddiTestun(ffrwd.ToArray(), true);
                gwybodaeth.CyfeiriadDelwedd = await _cronfaBeddargraff.ArbedDelwedd(ffrwd.ToArray());
            }

            return View(gwybodaeth);
        }

        [HttpPost]
        public async Task<IActionResult> ArbedBeddargraff(GwybodaethDelwedd gwybodaeth)
        {
            var beddargraff = new Beddargraff()
            {
                CyfeiriadDelwedd = gwybodaeth.CyfeiriadDelwedd,
                EnwBedd = gwybodaeth.EnwBedd,
                Mynwent = gwybodaeth.Mynwent,
                Dyddiad = gwybodaeth.Dyddiad,
                Llinellau = gwybodaeth.Llinellau
            };
            await _cronfaBeddargraff.ArbedBeddargraff(beddargraff);
            return RedirectToAction("RhestruBeddargrafiadau");
            //return View(beddargraff);
        }

        [HttpGet]
        public async Task<IActionResult> RhestruBeddargrafiadau()
        {
            var rhestrBeddargraffiadau = await _cronfaBeddargraff.AdalwBeddargraffiadau();
            return View(rhestrBeddargraffiadau);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
