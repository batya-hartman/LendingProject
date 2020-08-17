using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lender.Service
{
    public interface ILenderService
    {
        Task<bool> AddLender(Models.Lender Lender);
        Task<bool> CheckLendingPassible(Models.Lending lending);
    }
}
