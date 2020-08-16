using EInvest2.Models;
using System.Threading.Tasks;

namespace EInvest2.Service
{
    public interface ITesouroDireto
    {
        Task<TesouroDiretoResponse> Get();

    }
}

