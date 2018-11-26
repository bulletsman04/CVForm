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
    public class JobOffersApiController : ControllerBase
    {
        private readonly DataContext _context;

        public JobOffersApiController(DataContext context)
        {
            _context = context;
        }


        
        public ActionResult<List<JobOffer>> Offers()
        {
            List<JobOffer> searchResult = _context.JobOfers.Include(item => item.Company).ToList();
        
            return Ok(searchResult);
        }

       
        [HttpGet("{searchString}")]
        public ActionResult<List<JobOffer>> OffersSearch(string searchString)
        {
            List<JobOffer> searchResult = _context.JobOfers.Include(item => item.Company).ToList();
            if (String.IsNullOrEmpty(searchString))
                return NotFound();

            searchResult = searchResult.FindAll(item => item.JobTitle.Contains(searchString)).ToList();

        
            return Ok(searchResult);
        }
    }
}