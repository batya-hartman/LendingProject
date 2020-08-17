using Lending.Services;
using Lending.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Lendings.Data
{
    public class LendingRepository : ILendingRepository
    {
        private readonly LendingContext _lendingContext;

        public LendingRepository(LendingContext lendingContext)
        {
            _lendingContext = lendingContext;
        }
        public Task<Lender> GetLenderAsync(Guid lenderId)
        {
            return _lendingContext.Lenders.Include("Rules").FirstOrDefaultAsync(l => l.LenderId == lenderId);
        }
    }
}
