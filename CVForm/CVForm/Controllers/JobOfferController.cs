using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVForm.Controllers
{
    
    public class JobOfferController : Controller
    {

        public static List<Company> _companies = new List<Company>()
        {
            new Company(){ ID = 1, Name="P&G"},
            new Company(){ ID = 2, Name="Microsoft"},
            new Company(){ ID = 3, Name="Google"}
        };


        public static List<JobOffer> _jobOffers = new List<JobOffer>

        {
            new JobOffer{
                ID =1,
                JobTitle = "Backend Developer",
                Company = _companies.FirstOrDefault(c => c.Name =="P&G"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Backend C# developer with intrests about IoT solutions. The main task would be building API which expose data from phisical devices. Description need to have at least 100 characters so I am adding some. In test case I reccomend you to use Lorem Impsum.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            },
            new JobOffer{
                ID =2,
                JobTitle = "Frontend Developer",
                Company = _companies.FirstOrDefault(c => c.Name =="Microsoft"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Developing Office 365 front end interface. Working with SharePoint and graph API. Connecting with AAD and building ML for Mailbox smart assistant. Description need to have at least 100 characters so I am adding some. In test case I reccomend you to use Lorem Impsum.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            }
        };

    
        
        public IActionResult Index(string searchString)
        {
            if(String.IsNullOrEmpty(searchString))
                return View(_jobOffers);

            List<JobOffer> searchResult = _jobOffers.FindAll(item => item.JobTitle.Contains(searchString));
            return View(searchResult);
        }

        public IActionResult Details(int? id)
        {
            JobOffer selected = _jobOffers.FirstOrDefault(item => item.ID == id);

            return View(selected);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = _jobOffers.Find(item => item.ID == id);

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
            var offer = _jobOffers.Find(item => item.ID == model.ID);
            offer.JobTitle = model.JobTitle;
            return RedirectToAction("Details", new {id = model.ID});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id==null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _jobOffers.RemoveAll(item => item.ID == id);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView()
            {
                Companies = _companies
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _companies;
                return View(model);
            }

            var id = _jobOffers.Max(j => j.ID) + 1;
                
            _jobOffers.Add( new JobOffer()
            {
                ID = id,
                CompanyId = model.CompanyId,
                Company = _companies.FirstOrDefault(c => c.ID == model.CompanyId),
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            });

            return RedirectToAction("Index");
        }

}
}