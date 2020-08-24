using Lender.Service;
using Lender.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> EditLenderRulesAsync(Service.Models.Lender lender)
        {
            var oldLender = await GetLenderAsync(lender.LenderId);
            lender.RulesList.ForEach(rule => rule.LenderId = lender.LenderId);

            foreach (var rule in lender.RulesList)
            {
                var exist = oldLender.RulesList.FirstOrDefault(oldRule => oldRule.Description == rule.Description);
                if (exist != null)
                {
                    _lenderContext.Rules.Remove(exist);
                }

                _lenderContext.Rules.Add(rule);
            }
            return await _lenderContext.SaveChangesAsync() > 0;
        }

        public async Task<Service.Models.Lender> GetLenderAsync(Guid lenderId)
        {
            var res = await _lenderContext.Lenders.FirstOrDefaultAsync(l => l.LenderId == lenderId);
            res.RulesList = new List<Rule>();
            res.RulesList.AddRange(_lenderContext.Rules.Where(r => r.LenderId == lenderId));
            return res;
        }
    }
}
