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
        private readonly ICronfaEnglynion _cronfaEnglynion;

        public HomeController(IGwasanaethauGwybodol gwasanaethauGwybodol, ICronfaEnglynion cronfaEnlEnglynion)
        {
            _gwasanaethauGwybodol = gwasanaethauGwybodol;
            _cronfaEnglynion = cronfaEnlEnglynion;
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
                gwybodaeth.CyfeiriadDelwedd = await _cronfaEnglynion.ArbedDelwedd(ffrwd.ToArray());
            }

            return View(gwybodaeth);
        }

        [HttpPost]
        public async Task<IActionResult> ArbedBeddargraff(GwybodaethDelwedd gwybodaeth)
        {
            var englyn = new Englyn()
            {
                CyfeiriadDelwedd = gwybodaeth.CyfeiriadDelwedd,
                Bedd = gwybodaeth.EnwBedd,
                Mynwent = gwybodaeth.Mynwent,
                Dyddiad = gwybodaeth.Dyddiad,
                Llinell1 = gwybodaeth.Llinell1,
                Llinell2 = gwybodaeth.Llinell2,
                Llinell3 = gwybodaeth.Llinell3,
                Llinell4 = gwybodaeth.Llinell4,
                Bardd = gwybodaeth.Bardd
            };
            await _cronfaEnglynion.ArbedEnglyn(englyn);
            return RedirectToAction("RhestruBeddargrafiadau");
            //return View(beddargraff);
        }

        [HttpGet]
        public async Task<IActionResult> RhestruBeddargrafiadau()
        {
            var rhestrBeddargraffiadau = await _cronfaEnglynion.AdalwEnglynion();
            return View(rhestrBeddargraffiadau);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
