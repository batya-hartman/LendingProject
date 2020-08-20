using Microsoft.EntityFrameworkCore;

namespace Lendings.Data
{
    public class LendingContext : DbContext
    {
        public DbSet<Lending.Services.Models.Lender> Lenders { get; set; }
        public DbSet<Lending.Services.Models.Rule> Rules { get; set; }
        public DbSet<LendingEntity> LendingEntities { get; set; }
        public LendingContext(DbContextOptions<LendingContext> options)
   : base(options)
        { }
        public LendingContext()
        { }
    }
}
