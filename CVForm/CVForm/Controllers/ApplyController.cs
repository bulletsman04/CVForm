using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVForm.Controllers
{
    public class ApplyController : Controller
    {
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

            var id = JobOfferController._jobOffers[model.OfferId-1].JobApplications.Count == 0? 0 : JobOfferController._jobOffers[model.OfferId-1].JobApplications.Max(j => j.Id) + 1;

            JobOfferController._jobOffers[model.OfferId-1].JobApplications.Add(new JobApplication()
            {
                Id = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ContactAgreement = model.ContactAgreement,
                OfferId = model.OfferId
            });

            return RedirectToAction("Details","JobOffer",new {id = model.OfferId});
        }

        public async Task<IActionResult> Details(int offerId, int applicationId)
        {
            var application = JobOfferController._jobOffers[offerId - 1].JobApplications[applicationId];

            return View(application);
        }
    }
}