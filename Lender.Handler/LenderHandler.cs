using Lender.Service;
using Messages.Command;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Lender.Handler
{
    public class LenderHandler : IHandleMessages<CreateNewLender>
    {
        private readonly ILenderService _lenderService;
        public LenderHandler(ILenderService ilender)
        {
            _lenderService = ilender;
        }

        public Task Handle(CreateNewLender message, IMessageHandlerContext context)
        {
            Service.Models.Lender lender = new Service.Models.Lender()
            {
                Name = message.Name,
                PathToExcelFile = message.PathToExcelFile
            };
            return _lenderService.AddLender(lender);
        }
    }
}
