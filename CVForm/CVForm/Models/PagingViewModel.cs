using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVForm.Models
{
    public class PagingViewModel
    {
        public IEnumerable<JobOffer> Offers { get; set; }
        public int TotalPage { get; set; }
    }
}
