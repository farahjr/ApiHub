using System;
using System.Collections.Generic;
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
        public async Task<InvestimentosResponse> GetAsync()
        {
            var tesouro = new TesouroDiretoResponse();
            var rendaFixa = new RendaFixaResponse();
            var fundos = new FundosResponse();
            var investimentos = new InvestimentosResponse(new List<Investimento>());

            tesouro = await _tesouroDiretoService.Get();
            rendaFixa = await _rendaFixaService.Get();
            fundos = await _fundosService.Get();

            foreach (TesouroDireto t in tesouro.Tds)
            {
                investimentos.ValorTotal += t.ValorTotal;
                double ir = t.ValorInvestido - t.ValorTotal < 0 ? 0 : (t.ValorTotal - t.ValorInvestido) * 0.1;
                double resgate = t.ValorTotal;

                if (t.Vencimento < DateTimeOffset.UtcNow.AddMonths(3))                
                    resgate -= resgate * 0.06;                
                else if (t.Vencimento - DateTimeOffset.UtcNow < (t.DataDeCompra - t.Vencimento) / 2)                
                    resgate -= resgate * 0.15;                
                else                
                    resgate -= resgate * 0.3;

                investimentos.Investimentos
                    .Add(new Investimento(
                        t.Nome, t.ValorInvestido, t.ValorTotal, t.Vencimento, ir, resgate));
            }

            foreach (Lci r in rendaFixa.Lcis)
            {
                investimentos.ValorTotal += r.CapitalAtual;
                double ir = r.CapitalAtual - r.CapitalInvestido < 0 ? 0 : (r.CapitalAtual - r.CapitalInvestido) * 0.05;
                double resgate = r.CapitalAtual;

                if (r.Vencimento < DateTimeOffset.UtcNow.AddMonths(3))
                    resgate -= resgate * 0.06;
                else if (r.Vencimento - DateTimeOffset.UtcNow < (r.DataOperacao - r.Vencimento) / 2)
                    resgate -= resgate * 0.15;
                else
                    resgate -= resgate * 0.3;

                investimentos.Investimentos
                    .Add(new Investimento(
                        r.Nome, r.CapitalInvestido, r.CapitalAtual, r.Vencimento, ir, resgate));
            }

            foreach (Fundo f in fundos.Fundos)
            {
                investimentos.ValorTotal += f.ValorAtual;
                double ir = f.ValorAtual - f.CapitalInvestido < 0 ? 0 : (f.ValorAtual - f.CapitalInvestido) * 0.15;
                double resgate = f.ValorAtual;

                if (f.DataResgate < DateTimeOffset.UtcNow.AddMonths(3))
                    resgate -= resgate * 0.06;
                else if (f.DataResgate - DateTimeOffset.UtcNow < (f.DataCompra - f.DataResgate) / 2)
                    resgate -= resgate * 0.15;
                else
                    resgate -= resgate * 0.3;

                investimentos.Investimentos
                    .Add(new Investimento(
                        f.Nome, f.CapitalInvestido, f.ValorAtual, f.DataResgate, ir, resgate));
            }



            _logger.LogInformation("Buscando Tesouro Direto");
            return investimentos;
        }
    }
}
