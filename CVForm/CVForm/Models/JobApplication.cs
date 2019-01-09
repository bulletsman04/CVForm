using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVForm.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        // ToDO: TU nazwaliśmy niezgodnie z koncepcją, więc EF utwórzył FK do JobOfeer o nazwie JobOfferId
        public int OfferId { get; set; }
        [Required]
        [Display(Name = "First name: ")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name: ")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Phone number: ")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email address: ")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Contact agreement: ")]
        public bool ContactAgreement { get; set; }
        [Display(Name = "Link to CV: ")]
        public string CvUrl { get; set; }
    }
}
