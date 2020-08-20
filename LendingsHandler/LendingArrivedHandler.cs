using Lending.Services;
using Messages.Command;
using NServiceBus;
using NServiceBus.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LendingsHandler
{
    public class LendingArrivedHandler : IHandleMessages<LendingArrived>
    {
        private readonly ILendingService _lendingService;
        static readonly ILog _log = LogManager.GetLogger<LendingArrivedHandler>();

        public LendingArrivedHandler(ILendingService lendingService)
        {
            _lendingService = lendingService;
        }
        public Task Handle(LendingArrived message, IMessageHandlerContext context)
        {
            _log.Info("Lending arrived to LendindArrivedHandler");
            var res= _lendingService.CheckLendingPassibleAsync(MapToLendingModel(message));
            _log.Info($"Checked if the lending is ok, the result is {res.Result}");
            return res;
        }
        private Lending.Services.Models.Lending MapToLendingModel(LendingArrived lending)
        {
            Lending.Services.Models.Lending lending1 = new Lending.Services.Models.Lending()
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