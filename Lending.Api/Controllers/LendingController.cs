using Lending.Services;
using Messages.Command;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
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
        public LendingController(ILendingService lendingService,IMessageSession imessageSession)
        {
            _messageSession = imessageSession;
            _lendingService = lendingService;
        }
        [HttpPost]
        public async Task<bool> checkLendingPossible(LendingDTO lending)
        {
            Services.Models.Lending lendingModel = MapToLendingModel(lending);
            return await _lendingService.CheckLendingPassible(lendingModel);
        }
        [HttpPost("NSB")]
        public async Task<bool> checkLendingPossibleWithNSB(LendingDTO lending)
        {
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
        private Services.Models.Lending MapToLendingModel(LendingDTO lending)
        {
            Services.Models.Lending lending1 = new Services.Models.Lending()
            {
                LenderId = lending.LenderId,
                PrincipalSignature = lending.PrincipalSignature,
                Parameters = new Dictionary<string, object>()
            };
           
            foreach (var item in lending.BoolParameters)
            {
                lending1.Parameters.Add(item.Key, item.Value);
            }
            foreach (var item in lending.doubleParameters)
            {
                lending1.Parameters.Add(item.Key, item.Value);
            }
            foreach (var item in lending.StringParameters)
            {
                lending1.Parameters.Add(item.Key, item.Value);
            }
            return lending1;
        } 
    }
}
