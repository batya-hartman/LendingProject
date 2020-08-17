using Lending.Services.Models;
using System;
using System.Threading.Tasks;

namespace Lending.Services
{
    public interface ILendingRepository
    {
        Task<Lender> GetLenderAsync(Guid lenderId);
    }
}
