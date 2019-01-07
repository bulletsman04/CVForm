using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CVForm.Controllers
{
    public class ApplyController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _configuration;
        public ApplyController(DataContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }
        public async Task<ActionResult> Index(int id)
        {
            var jobApplication = new JobApplicationViewModel()
            {
                OfferId = id
            };
            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(JobApplicationViewModel model)
        {
            //ToDo: Accept only pdf
            if (!ModelState.IsValid)
            {
               return View(model);
            }
            
            string cvFileName = "";
            CloudStorageAccount storageAccount = null;
            if (CloudStorageAccount.TryParse(_configuration["ConnectionStrings:BlobStorage"], out storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

               
                // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(_configuration["OtherStrings:BlobContainerReference"]);
                //await container.CreateIfNotExistsAsync();

                //ToDo: Better name and then change in flow to display appriopriately in sharepoint - firstly logged user id?
                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(model.CvFile.FileName);

                // Upload the file
                using (var stream = model.CvFile.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                cvFileName = blockBlob.Uri.AbsoluteUri;
            }



            JobApplication jobApplication =  new JobApplication()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                ContactAgreement = model.ContactAgreement,
                OfferId = model.OfferId,
                CvUrl = cvFileName
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