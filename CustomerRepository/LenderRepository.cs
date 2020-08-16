using Lender.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Lender.Data
{
    public class LenderRepository : ILenderRepository
    {
        private readonly LenderContext _lenderContext;
        public LenderRepository(LenderContext lenderContext)
        {
            _lenderContext = lenderContext;
        }

        public async Task<bool> AddLenderAsync(Service.Models.Lender lender)
        {
            if (String.IsNullOrEmpty(lender.Name) || lender.RulesList.Count == 0)
            {
                return false;
            }
            lender.LenderId = Guid.NewGuid();
            _lenderContext.Lenders.Add(lender);
            return await _lenderContext.SaveChangesAsync() > 0;
        }
        public Task<Service.Models.Lender> GetLenderAsync(Guid lenderId)
        {
           return _lenderContext.Lenders.FirstOrDefaultAsync(l => l.LenderId == lenderId);
        }
    }
}
