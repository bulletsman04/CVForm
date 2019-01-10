using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        /// Gets all job applications for the offer
        /// </summary>
        /// <param name="offerId">Id of the offer</param>
        /// <returns></returns>
        [HttpGet("{offerId}")]
        public ActionResult<List<JobApplication>> OffersSearch(int offerId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<JobApplication> searchResult =
                _context.JobOfers.Include(item => item.JobApplications).FirstOrDefault(item => item.ID == offerId ).JobApplications.FindAll(application =>  application.UserId == userId);
           
            return Ok(searchResult);
        }
    }
}