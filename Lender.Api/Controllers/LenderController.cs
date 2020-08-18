using Lender.Service;
using Lender.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            return _lenderService.AddLender(lender);
        }
    }
}
