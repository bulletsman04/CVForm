using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVForm.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        // ToDO: TU nazwaliśmy niezgodnie z koncepcją, więc EF utwórzył FK do JobOfeer o nazwie JobOfferId
        public int OfferId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
    }
}
