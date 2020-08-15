using EInvest2.Models;
using System.Threading.Tasks;

namespace EInvest2.Interface
{
    public interface IRendaFixa
    {
        Task<RendaFixaModel> Get();

    }
}
