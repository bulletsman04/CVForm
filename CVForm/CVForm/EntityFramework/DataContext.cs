using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVForm.Models;
using Microsoft.EntityFrameworkCore;

namespace CVForm.EntityFramework
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobOffer> JobOfers { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
