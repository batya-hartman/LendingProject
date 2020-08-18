using System.Threading.Tasks;

namespace Lending.Services
{
    public interface ISendLending
    {
        Task<bool> SendToCheckLendingPassible(Models.Lending lending);
    }
}