using Lending.Services;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System.Threading.Tasks;

namespace Lending.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendingController : ControllerBase
    {
        private readonly IMessageSession _messageSession;
        private readonly ILendingService _lendingService;
        public LendingController(ILendingService lendingService,IMessageSession imessageSession)
        {
            _messageSession = imessageSession;
            _lendingService = lendingService;
        }
        [HttpPost]
        public async Task<bool> checkLendingPossible(Services.Models.Lending lending)
        {
            await _lendingService.CheckLendingPassible(lending);
            return true;
        }
        [HttpPost("NSB")]
        public async Task<bool> checkLendingPossibleWithNSB(Services.Models.Lending lending)
        {
            await _messageSession.Send(lending);
            return true;
        }
    }
}
