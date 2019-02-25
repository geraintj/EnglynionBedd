using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnglynionBedd.Endidau;
using EnglynionBedd.Gwasanaethau;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglynionBedd.Controllers
{
    [Authorize]
    public class EnglynController : Controller
    {
        private readonly ICronfaEnglynion _cronfaEnglynion;

        public EnglynController(ICronfaEnglynion cronfaEnglynion)
        {
            _cronfaEnglynion = cronfaEnglynion;
        }

        [HttpGet]
        public async Task<IActionResult> RhestruEnglynion()
        {
            var rhestrEnglynion = await _cronfaEnglynion.AdalwEnglynion();
            return View(rhestrEnglynion);
        }

        // GET: Englyn/Manylion/5
        public async Task<IActionResult> Manylion(string id)
        {
            var englyn = await _cronfaEnglynion.AdalwEnglyn(id);
            return View(englyn);
        }

        // GET: Englyn/Golygu/5
        public async Task<IActionResult> Golygu(string id)
        {
            var englyn = await _cronfaEnglynion.AdalwEnglyn(id);
            return View(englyn);
        }

        // POST: Englyn/Golygu/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Golygu(Englyn englyn)
        {
            await _cronfaEnglynion.GolyguEnglyn(englyn);
            return RedirectToAction(nameof(RhestruEnglynion));
        }

        // GET: Englyn/Diddymu/5
        public ActionResult Diddymu(int id)
        {
            return View();
        }

        // POST: Englyn/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Diddymu(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(RhestruEnglynion));
            }
            catch
            {
                return View();
            }
        }
    }
}