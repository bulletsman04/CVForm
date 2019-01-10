using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CVForm.Models
{
    public class JobOffer: IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }

        public virtual Company Company { get; set; }
        public virtual int CompanyId { get; set; }

        [MinValue(0)]
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }

        [MinValue(0)]
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime Created { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        [Display(Name = "Valid until")]
        [NotPastDate]
        public DateTime? ValidUntil { get; set; }

        [ForeignKey("OfferId")]
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SalaryFrom > SalaryTo)
            {
                yield return new ValidationResult(
                    "Upper bound of salary must be bigger than lower bound.",
                    new[] { "SalaryFrom" });
            }
        }
    }
}
