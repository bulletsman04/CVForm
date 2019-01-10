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
        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }

        public JobOfferController(DataContext context,IConfiguration configuration)
        {
            _configuration = configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
            _context = context;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[controller]/Details/{id?}")]
        public IActionResult Details(int? id)
        {
            JobOffer selected = _context.JobOfers.Include(item => item.Company).Include(item => item.JobApplications).FirstOrDefault(item => item.ID == id);

            if (selected == null)
                //  return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                return NotFound();
            return View(selected);
        }

        [Authorize]
        [HttpGet("[controller]/Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            AADGraph graph = new AADGraph(AppSettings);
            string groupName = "Admins";
            string groupId = AppSettings.AADGroups.FirstOrDefault(g =>
                String.Compare(g.Name, groupName) == 0).Id;
            bool isIngroup = await graph.IsUserInGroup(User.Claims, groupId);

            if (!isIngroup)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }


            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = _context.JobOfers.Include(item => item.Company).FirstOrDefault(item => item.ID == id);

            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
                return View();
            var offer = _context.JobOfers.FirstOrDefault(item => item.ID == model.ID);
            if (offer != null)
            {
                offer.CompanyId = model.CompanyId;
                offer.Description = model.Description;
                offer.JobTitle = model.JobTitle;
                offer.Location = model.Location;
                offer.SalaryFrom = model.SalaryFrom;
                offer.SalaryTo = model.SalaryTo;
                offer.ValidUntil = model.ValidUntil;
                
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
            //  IValidable object
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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}