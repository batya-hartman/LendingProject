using System.Threading.Tasks;

namespace Lending.Services
{
    public interface ILendingService
    {
        Task<bool> CheckLendingPassible(Models.Lending lending);
    }
}
