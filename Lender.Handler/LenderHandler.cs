using Lender.Service;
using System;
using System.Threading.Tasks;

namespace Lender.Handler
{
    public class LenderHandler
    {
        private readonly ILenderService _lenderService;
        public LenderHandler(ILenderService ilender)
        {
            _lenderService = ilender;
        }
        public Task<bool> AddLenderAsync(Lender.Service.Models.Lender lender)
        {
            return _lenderService.AddLender(lender);
        }
        public Task<Lender.Service.Models.Lender> GetLenderAsync(Guid lenderId)
        {
            throw new NotImplementedException();
        }
    }
}
