using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommunityCertForT;
using CommunityCertForT.Helpers;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[controller]/Details/{id?}")]
        [Authorize]
        public IActionResult Details(int? id)
        {

            JobOffer selected = _context.JobOfers.Include(item => item.Company).Include(item => item.JobApplications).FirstOrDefault(item => item.ID == id);

            if (selected == null)
                return NotFound();
            return View(selected);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("[controller]/Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null)
                return BadRequest();
            var offer = _context.JobOfers.Include(item => item.Company).FirstOrDefault(item => item.ID == id);

            if (offer == null)
                return NotFound();


            var offerEditView = new JobOfferCreateView()
            {
                ID = offer.ID,
                CompanyId = offer.CompanyId,
                Description = offer.Description,
                Location = offer.Location,
                SalaryFrom = offer.SalaryFrom,
                JobTitle = offer.JobTitle,
                SalaryTo = offer.SalaryTo,
                ValidUntil = offer.ValidUntil,
                Created = offer.Created,
                Company = offer.Company,
                Companies = _context.Companies.ToList()
        };

            return View(offerEditView);
        }

        [HttpPost("[controller]/Edit/{id?}")]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
                return View();
            var offer = _context.JobOfers.FirstOrDefault(item => item.ID == model.ID);
            var test = "0";
            if (offer != null)
            {
                test = "1";
                offer.CompanyId = model.CompanyId;
                offer.Description = model.Description;
                offer.JobTitle = model.JobTitle;
                offer.Location = model.Location;
                offer.SalaryFrom = model.SalaryFrom;
                offer.SalaryTo = model.SalaryTo;
                offer.ValidUntil = model.ValidUntil;
                
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new {id = test});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id==null)
                return BadRequest();

            _context.RemoveRange(_context.JobOfers.Where(item => item.ID == id));
          
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [Authorize(Policy = "Admin")]
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
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _context.Companies.ToList();
                return View(model);
            }

            
                JobOffer jobOffer = new JobOffer()
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
          

            _context.JobOfers.Add(jobOffer);
            await _context.SaveChangesAsync();
       
            return RedirectToAction("Index");
        }



    }
}