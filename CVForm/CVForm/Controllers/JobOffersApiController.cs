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
        [HttpGet("{pageNumber}")]
        public ActionResult<List<JobOffer>> GetAll(int pageNumber = 1)
        {
            PagingViewModel pagedOffers = PreparePagingViewModel(pageNumber);

            return Ok(pagedOffers);
        }

        /// <summary>
        /// Gets all the job offers that have searchString in name
        /// </summary>
        /// <returns>All job offers</returns>
        [HttpGet("{searchString}/{pageNumber}")]
        public ActionResult<List<JobOffer>> OffersSearch(string searchString, int pageNumber = 1)
        {
            List<JobOffer> searchResult = _context.JobOfers.Include(item => item.Company).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchResult = searchResult.FindAll(item => item.JobTitle.Contains(searchString)).ToList();
            }

            PagingViewModel pagedOffers = PreparePagingViewModel(pageNumber);

            return Ok(pagedOffers);
        }

        private PagingViewModel PreparePagingViewModel(int pageNumber)
        {
            List<JobOffer> searchResult = _context.JobOfers.Include(item => item.Company).ToList();

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
            return pagedOffers;
        }

    }
}