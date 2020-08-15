using System.Threading.Tasks;
using EInvest2.Models;
using EInvest2.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EInvest2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarteiraController : ControllerBase
    {
        private readonly ILogger<CarteiraController> _logger;
        private TesouroDiretoService tesouroDiretoService = new TesouroDiretoService();

        public CarteiraController(ILogger<CarteiraController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<TesouroDiretoModel> GetAsync()
        {
            _logger.LogInformation("Buscando Tesouro Direto");
            return await tesouroDiretoService.Get();
        }
    }
}
