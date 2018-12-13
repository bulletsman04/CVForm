using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CVForm.Controllers
{
    
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
        }


        [Authorize]
        public IActionResult Index(string searchString)
        {
            List<JobOffer> searchResult = _context.JobOfers.Include(item => item.Company).ToList();
            if (String.IsNullOrEmpty(searchString))
                return View(searchResult);

             searchResult = searchResult.FindAll(item => item.JobTitle.Contains(searchString)).ToList();
            return View(searchResult);
        }

        public IActionResult Details(int? id)
        {
            JobOffer selected = _context.JobOfers.Include(item => item.Company).Include(item => item.JobApplications).FirstOrDefault(item => item.ID == id);

            return View(selected);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = _context.JobOfers.Include(item => item.Company).FirstOrDefault(item => item.ID == id);

            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
                return View();
            var offer = _context.JobOfers.FirstOrDefault(item => item.ID == model.ID);
            if (offer != null)
            {
                offer.JobTitle = model.JobTitle;
                offer.Description = model.Description;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new {id = model.ID});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id==null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.RemoveRange(_context.JobOfers.Where(item => item.ID == id));
          
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView()
            {
                Companies = _context.Companies.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _context.Companies.ToList();
                return View(model);
            }

         
           JobOffer jobOffer =  new JobOffer()
            {
                
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };
            // jquery validate ; IValidable object
            try
            {
                _context.JobOfers.Add(jobOffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                model.Companies = _context.Companies.ToList();
                return View(model);
            }

            return RedirectToAction("Index");
        }

}
}