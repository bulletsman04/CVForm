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
    [Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobOffersApiController : ControllerBase
    {
        private readonly DataContext _context;

        public JobOffersApiController(DataContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Gets all the job offers
        /// </summary>
        /// <returns>All job offers</returns>

        //[HttpGet("{pageNumber}/{searchString}")]
        [HttpGet]
        public ActionResult<List<JobOffer>> OffersSearch(int pageNumber = 1, string searchString="")
        {
            // ToDo: Maybe divide again to two actions and extract common paging functionality

            List<JobOffer> searchResult = _context.JobOfers.Include(item => item.Company).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchResult = searchResult.FindAll(item => item.JobTitle.Contains(searchString)).ToList();
            }

            int totalPage, totalRecord, pageSize;
            pageSize = 4;
            totalRecord = searchResult.Count;
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

            searchResult = searchResult.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            PagingViewModel pagedOffers = new PagingViewModel
            {
                Offers = searchResult,
                TotalPage = totalPage
            };

            return Ok(pagedOffers);
        }
    }
}