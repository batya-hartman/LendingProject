using Lending.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Lendings.Data
{
    public class LendingContext : DbContext
    {
        public DbSet<Lender> Lenders { get; set; }
        public DbSet<Rule> Rules { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //if (!optionsBuilder.IsConfigured)
        //    //{
                 
        //        optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["LendingConnection"]);
        //        base.OnConfiguring(optionsBuilder);
        //   // }
        //}
        public LendingContext(DbContextOptions<LendingContext> options)
   : base(options)
        { }
        public LendingContext()
        { }
    }
}
