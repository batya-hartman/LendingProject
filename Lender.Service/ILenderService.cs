using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lender.Service
{
    public interface ILenderService
    {
        Task<bool> AddLenderAsync(Models.Lender Lender);
        Task<bool> EditLenderAsync(Models.Lender lender);
    }
}
