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
        private TesouroDiretoService _tesouroDiretoService = new TesouroDiretoService();
        private RendaFixaService _rendaFixaService = new RendaFixaService();
        private FundosService _fundosService = new FundosService();

        public CarteiraController(ILogger<CarteiraController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<TesouroDiretoResponse> GetAsync()
        {
            var tesouro = new TesouroDiretoResponse();
            var rendaFixa = new RendaFixaResponse();
            var fundos = new FundosResponse();
            var investimentos = new InvestimentosResponse();

            tesouro = await _tesouroDiretoService.Get();
            rendaFixa = await _rendaFixaService.Get();
            fundos = await _fundosService.Get();

            //foreach(TesouroDireto t in tesouro.Tds)
            //{
            //    investimentos.ValorTotal += t.ValorTotal;
            //    investimentos.Investimentos.Add(new Investimento(t.Nome,t.ValorInvestido,t.ValorTotal, t.Vencimento, (t.ValorTotal - t.ValorInvestido) * 0,1));
            //}

            //foreach (Lci r in rendaFixa.Lcis)
            //{
            //    investimentos.ValorTotal += r.ValorTotal;
            //    investimentos.Investimentos.Add(new Investimento(r.Nome, r.CapitalInvestido, r., r.Vencimento, (r.ValorTotal - r.ValorInvestido) * 0, 1));
            //}

            //foreach (Fundo f in fundos.Fundos)
            //{
            //    investimentos.ValorTotal += f.ValorTotal;
            //    investimentos.Investimentos.Add(new Investimento(f.Nome, f.ValorInvestido, f.ValorTotal, f.Vencimento, (f.ValorTotal - f.ValorInvestido) * 0, 1));
            //}



            _logger.LogInformation("Buscando Tesouro Direto");
            return tesouro;
        }
    }
}
