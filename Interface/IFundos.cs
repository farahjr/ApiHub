using EInvest2.Models;
using System.Threading.Tasks;

namespace EInvest2.Interface
{
    public interface IFundos
    {
        Task<FundosResponse> Get();

    }
}
