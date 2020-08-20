using System.Threading.Tasks;

namespace Lending.Services
{
    public interface ILendingService
    {
        Task<bool> CheckLendingPassibleAsync(Models.Lending lending);
    }
}
