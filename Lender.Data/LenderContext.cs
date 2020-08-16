using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace LendingData
{
    public class LenderContext : DbContext
    {
        public DbSet<Lender> Lenders { get; set; }
        public DbSet<Rule> Rules { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
                 
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["LendingConnection"]);
                base.OnConfiguring(optionsBuilder);
           // }
        }
        public LenderContext(DbContextOptions<LenderContext> options)
   : base(options)
        { }
        public LenderContext()
        { }
    }
}
