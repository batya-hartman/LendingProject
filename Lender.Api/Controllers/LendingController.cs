using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lender.Service;
using Lending.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lender.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendingController : ControllerBase
    {
        private readonly ILendingService _lendingService;
        private readonly ILenderService _lenderService;
        public LendingController(ILenderService ilenderService,ILendingService lendingService)
        {
            _lendingService = lendingService;
            _lenderService = ilenderService;
        }
        [HttpPost("try")]
        public async Task<bool> trying(Lending.Services.Models.Lending lending)
        {
            await _lendingService.CheckLendingPassible(lending);
            return true;
        }
        [HttpPost]
        public async Task<bool> CheckLendingPassible(Service.Models.Lending lending)
        {
            return await _lenderService.CheckLendingPassible(lending);
        }
    }
}
