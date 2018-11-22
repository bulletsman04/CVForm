using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVForm.Controllers
{
    public class ApplyController : Controller
    {
        private readonly DataContext _context;

        public ApplyController(DataContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index(int id)
        {
            var jobApplication = new JobApplication()
            {
                OfferId = id
            };
            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(JobApplication model)
        {
            if (!ModelState.IsValid)
            {
               return View(model);
            }

            JobApplication jobApplication =  new JobApplication()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ContactAgreement = model.ContactAgreement,
                OfferId = model.OfferId
            };

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","JobOffer",new {id = model.OfferId});
        }

        public async Task<IActionResult> Details(int offerId, int applicationId)
        {
            var application = _context.JobApplications.FirstOrDefault(item => item.Id == applicationId);

            return View(application);
        }
    }
}