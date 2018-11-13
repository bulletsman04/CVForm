using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVForm.Controllers
{
    public class CompaniesController : Controller
    {
        public IActionResult Index()
        {
            return View(JobOfferController._companies);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = JobOfferController._companies.Find(item => item.ID == id);

            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company model)
        {
            if (!ModelState.IsValid)
                return View();
            var offer = JobOfferController._companies.Find(item => item.ID == model.ID);
            offer.Name = model.Name;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            JobOfferController._companies.RemoveAll(item => item.ID == id);

            return RedirectToAction("Index");
        }
    }
}