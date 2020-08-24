using Lender.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LenderController : Controller
    {
        private readonly ILenderService _lenderService;
        public LenderController(ILenderService ilenderService)
        {
            _lenderService = ilenderService;
        }

        [HttpPut]
        public Task<bool> AddLender(Service.Models.Lender lender)
        {
            if (String.IsNullOrEmpty(lender.Name))
            {
                throw new Exception("Name can't be null or empty");
            }
            var res = _lenderService.AddLenderAsync(lender);
            return res;
        }
        [HttpPost]
        public Task<bool> EditLenderRules(Service.Models.Lender lender)
        {
            if (lender.LenderId == Guid.Empty)
            throw new Exception("LenderId can't be empty");
            return _lenderService.EditLenderAsync(lender);
        }
    }
}
