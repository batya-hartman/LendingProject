using Lending.Services;
using Messages.Command;
using NServiceBus;
using NServiceBus.Logging;
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
            var lending = new Lending.Services.Models.Lending()
            {
                LenderId = message.LenderId,
        //        Parameters = message.Parameters,
                PrincipalSignature = message.PrincipalSignature
            };
            return _lendingService.CheckLendingPassible(lending);
        }
    }
}