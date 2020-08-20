using Lending.Services;
using Messages.Command;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lending.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendingController : ControllerBase
    {
        private readonly IMessageSession _messageSession;
        private readonly ILendingService _lendingService;
        public LendingController(
            ILendingService lendingService,
            IMessageSession imessageSession)
        {
            _messageSession = imessageSession;
            _lendingService = lendingService;
        }

        [HttpPost("NSB")]
        public async Task<bool> checkLendingPossibleWithNSBAsync(LendingDTO lending)
        {
            if (lending.StringParameters.Count + lending.doubleParameters.Count + lending.BoolParameters.Count == 0)
            {
                throw new System.Exception("No parameters were provided");
            }
            LendingArrived lendingArrived = new LendingArrived()
            {
                LenderId = lending.LenderId,
                PrincipalSignature = lending.PrincipalSignature,
                BoolParameters = lending.BoolParameters,
                doubleParameters = lending.doubleParameters,
                StringParameters = lending.StringParameters
            };
            await _messageSession.Send(lendingArrived);
            return true;
        }
        [HttpPost]
        public async Task<bool> checkLendingPossibleAsync(LendingDTO lending)
        {
            Services.Models.Lending lendingModel = MapToLendingModel(lending);
            return await _lendingService.CheckLendingPassibleAsync(lendingModel);
        }
        private Services.Models.Lending MapToLendingModel(LendingDTO lending)
        {
            Services.Models.Lending lendingModel = new Services.Models.Lending()
            {
                LenderId = lending.LenderId,
                PrincipalSignature = lending.PrincipalSignature,
                Parameters = new Dictionary<string, object>()
            };

            foreach (var item in lending.BoolParameters)
            {
                lendingModel.Parameters.Add(item.Key, item.Value);
            }
            foreach (var item in lending.doubleParameters)
            {
                lendingModel.Parameters.Add(item.Key, item.Value);
            }
            foreach (var item in lending.StringParameters)
            {
                lendingModel.Parameters.Add(item.Key, item.Value);
            }
            return lendingModel;
        }
    }
}
