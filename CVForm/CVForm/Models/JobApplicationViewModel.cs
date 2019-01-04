using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CVForm.Models
{
    public class JobApplicationViewModel: JobApplication
    {
        public IFormFile CvFile { get; set; }

    }
}
