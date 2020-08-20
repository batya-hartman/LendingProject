using Microsoft.EntityFrameworkCore;

namespace Lender.Data
{
    public class LenderContext : DbContext
    {
        public DbSet<Service.Models.Lender> Lenders { get; set; }
        public DbSet<Service.Models.Rule> Rules { get; set; }

        public LenderContext(DbContextOptions<LenderContext> options)
       : base(options)
        { }
        public LenderContext()
        { }
    }
}
