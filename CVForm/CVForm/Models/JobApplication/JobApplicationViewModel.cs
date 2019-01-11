using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CVForm.Models
{
    public class JobApplicationViewModel: JobApplication
    {
        [Required]
        public IFormFile CvFile { get; set; }

    }
}
