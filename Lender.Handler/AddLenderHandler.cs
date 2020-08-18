using Lender.Service;
using Messages.Command;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Rules.Handler
{
    public class AddLenderHandler : IHandleMessages<CreateNewLender>
    {
        private readonly ILenderService _lenderService;
        public AddLenderHandler(ILenderService ilender)
        {
            _lenderService = ilender;
        }

        public Task Handle(CreateNewLender message, IMessageHandlerContext context)
        {     
           // return _lenderService.AddLender(lender);
        }
    }
}
