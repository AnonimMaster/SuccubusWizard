using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuccubusWizard.Models
{
	public class IncubusContext : DbContext
	{
        public DbSet<IncubusData> IncubusList { get; set; }
        public IncubusContext(DbContextOptions<IncubusContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
