﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CVForm.Controllers
{
    public class ApplyController : Controller
    {
        
        private readonly DataContext _context;
        private IConfiguration _configuration;
        private IAuthorizationService _authorizationService;
        public ApplyController(DataContext context, IConfiguration Configuration, IAuthorizationService iAuthorizationService)
        {
            _authorizationService = iAuthorizationService;   
            _context = context;
            _configuration = Configuration;
        }

        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(_context.JobApplications.ToList());
        }

        public async Task<ActionResult> Create(int id)
        {
            var jobApplication = new JobApplicationViewModel()
            {
                OfferId = id
            };
            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobApplicationViewModel model)
        {
            //ToDo: Accept only pdf
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            JobApplication jobApplicationDb =
                _context.JobApplications.FirstOrDefault(application => application.UserId == userId && application.OfferId == model.OfferId);
            if (jobApplicationDb != null)
            {
                _context.JobApplications.Remove(jobApplicationDb);
            }

            string cvFileName = await SaveCVToStorage(model);

            JobApplication jobApplication = new JobApplication()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ContactAgreement = model.ContactAgreement,
                OfferId = model.OfferId,
                CvUrl = cvFileName,
                UserId = userId
            };
            

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "JobOffer", new { id = model.OfferId });
        }


        public async Task<IActionResult> Details(int offerId, int applicationId)
        {
            var application = _context.JobApplications.FirstOrDefault(item => item.Id == applicationId);

            if (application == null)
            {
                return NotFound();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (application.UserId != userId && !(await _authorizationService.AuthorizeAsync(User, "Admin")).Succeeded)
            {
                return Unauthorized();
            }

            JobAppicationDetailsViewModel jobAppicationDetailsViewModel = new JobAppicationDetailsViewModel()
            {
                FirstName = application.FirstName,
                LastName = application.LastName,
                EmailAddress = application.EmailAddress,
                ContactAgreement = application.ContactAgreement,
                CvUrl = application.CvUrl,
                Id = applicationId,
                OfferId = application.OfferId,
                PhoneNumber = application.PhoneNumber,
                JobOffer = _context.JobOfers.Include(item => item.Company).FirstOrDefault(item => item.ID == offerId)
            };



            return View(jobAppicationDetailsViewModel);
        }


        private async Task<string> SaveCVToStorage(JobApplicationViewModel model)
        {
            string cvFileName = "";
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CloudStorageAccount storageAccount = null;
            if (CloudStorageAccount.TryParse(_configuration["ConnectionStrings:BlobStorage"], out storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = cloudBlobClient.GetContainerReference(_configuration["OtherStrings:BlobContainerReference"]);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(userId + "_" + model.OfferId + "_CV.pdf");

                using (var stream = model.CvFile.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                cvFileName = blockBlob.Uri.AbsoluteUri;
            }

            return cvFileName;
        }
    }
}