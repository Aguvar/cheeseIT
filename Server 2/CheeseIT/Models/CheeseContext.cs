using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.Models
{
    public class CheeseContext : DbContext
    {
        public CheeseContext(DbContextOptions<CheeseContext> options )
            : base(options)
        {

        }

        public DbSet<Cheese> Cheeses { get; set; }
        public DbSet<Ripening> Ripenings { get; set; }
        public DbSet<Experiment> Experiments { get; set; }

    }
}
