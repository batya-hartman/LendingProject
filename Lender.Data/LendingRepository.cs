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
        public async Task<bool> AddLendingToDBAsync(Lending.Services.Models.Lending lending, bool succeeded)
        {
            LendingEntity lendingEntity = new LendingEntity()
            {
                LenderId = lending.LenderId,
                PrincipalSignature = lending.PrincipalSignature,
                Confirmed = succeeded,
                Parameters = new System.Collections.Generic.List<Parameter>()
            };
            foreach (var item in lending.Parameters)
            {
                var p = new Parameter()
                {
                    Value = item.Key + " = " + item.Value.ToString()
                };
                lendingEntity.Parameters.Add(p);
            }
            _lendingContext.LendingEntities.Add(lendingEntity);
            return await _lendingContext.SaveChangesAsync() > 0;
        }
        public async Task<Lender> GetLenderAsync(Guid lenderId)
        {
            return await _lendingContext.Lenders.Include("Rules").FirstOrDefaultAsync(l => l.LenderId == lenderId);
        }
    }
}
