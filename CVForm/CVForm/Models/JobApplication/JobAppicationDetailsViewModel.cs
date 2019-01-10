using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVForm.Models
{
    public class JobAppicationDetailsViewModel: JobApplication
    {
        public JobOffer JobOffer { get; set; }
    }
}
