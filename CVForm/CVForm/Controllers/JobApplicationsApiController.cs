using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVForm.EntityFramework;
using CVForm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CVForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsApiController : ControllerBase
    {
        private readonly DataContext _context;

        public JobApplicationsApiController(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all the job offers
        /// </summary>
        /// <returns>All job offers</returns>
        [HttpGet("{offerId}")]
        public ActionResult<List<JobApplication>> OffersSearch(int offerId)
        {
            List<JobApplication> searchResult =
                _context.JobOfers.Include(item => item.JobApplications).FirstOrDefault(item => item.ID == offerId).JobApplications;
           
            return Ok(searchResult);
        }
    }
}