using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVForm.Controllers
{
    
    public class JobOfferController : Controller
    {
        public static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer(){ID = 1,JobTitle = "Seller"},
            new JobOffer(){ID = 2,JobTitle = "Frontend Developer"},
            new JobOffer(){ID = 3,JobTitle = "Backend Developer"},
            new JobOffer(){ID = 4,JobTitle = "Manager"},
            new JobOffer(){ID = 5,JobTitle = "Teacher"}
        };

        
        public IActionResult Index()
        {
            return View(_jobOffers);
        }

        public IActionResult Details(int? id)
        {
            JobOffer selected = _jobOffers.FirstOrDefault(item => item.ID == id);

            return View(selected);
        }
    }
}