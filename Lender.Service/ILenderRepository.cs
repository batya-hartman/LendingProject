using System;
using System.Threading.Tasks;

namespace Lender.Service
{
    public interface ILenderRepository
    {
        Task<bool> AddLenderAsync(Models.Lender lender);
        Task<Models.Lender> GetLenderAsync(Guid lenderId);
    }
}
