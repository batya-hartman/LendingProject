using System.Threading.Tasks;

namespace Lending.Services
{
    public interface ILendingService
    {
        Task CheckLendingPassible(Lending.Services.Models.Lending lending);
    }
}
