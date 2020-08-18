using Messages.Command;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Lending.Services
{
    public class SendLending : ISendLending
    {
        private readonly IMessageSession _messageSession;

        public SendLending(IMessageSession imessageSession)
        {
            _messageSession = imessageSession;
        }
        public async Task<bool> SendToCheckLendingPassible(Models.Lending lending)
        {
            if (lending.LenderId != Guid.Empty &&
                lending.StringParameters.Count + lending.doubleParameters.Count + lending.BoolParameters.Count > 0)
            {
                LendingArrived lendingArrived = new LendingArrived()
                {
                    LenderId = lending.LenderId,
                    PrincipalSignature = lending.PrincipalSignature,
                    BoolParameters = lending.BoolParameters,
                    doubleParameters = lending.doubleParameters,
                    StringParameters = lending.StringParameters
                };
                await _messageSession.Send(lendingArrived).ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}
