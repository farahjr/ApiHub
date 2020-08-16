using EInvest2.Models;
using EInvest2.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EInvest2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarteiraController : ControllerBase
    {
        private const double taxaInvestimentoTesouroDireto = 0.1;
        private const double taxaInvestimentoRendaFixa = 0.05;
        private const double taxaInvestimentoFundos = 0.15;

        private const double taxaResgateOutros = 0.3;
        private const double taxaResgateAteTresMeses = 0.15;
        private const double taxaResgateMaiorQueMeioPeriodo = 0.06;

        private readonly ILogger<CarteiraController> _logger;
        private readonly TesouroDiretoService _tesouroDiretoService = new TesouroDiretoService();
        private RendaFixaService _rendaFixaService = new RendaFixaService();
        private FundosService _fundosService = new FundosService();

        public CarteiraController(ILogger<CarteiraController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<InvestimentosResponse> GetAsync()
        {
            var investimentos = new InvestimentosResponse();
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            TesouroDiretoResponse tesouro = await _tesouroDiretoService.Get();
            RendaFixaResponse rendaFixa = await _rendaFixaService.Get();
            FundosResponse fundos = await _fundosService.Get();

            monstaInvestimentoResponse(investimentos, utcNow, tesouro, rendaFixa, fundos);
            _logger.LogInformation("Buscando Tesouro Direto");
            return investimentos;
        }

        private static void monstaInvestimentoResponse(InvestimentosResponse investimentos, DateTimeOffset utcNow, TesouroDiretoResponse tesouro, RendaFixaResponse rendaFixa, FundosResponse fundos)
        {
            foreach (var investimento in tesouro.Tds)
            {
                investimentos.ValorTotal += investimento.ValorTotal;
                double ir = CalcularIr(investimento.ValorInvestido, investimento.ValorTotal, taxaInvestimentoTesouroDireto);
                double resgate = investimento.ValorTotal;
                resgate = CalcularResgateComDesconto(utcNow, investimento.DataDeCompra, investimento.Vencimento, resgate);
                investimentos.Investimentos
                    .Add(new Investimento()
                    {
                        Nome = investimento.Nome,
                        ValorInvestido = investimento.ValorInvestido,
                        ValorTotal = investimento.ValorTotal,
                        Vencimento = investimento.Vencimento,
                        Ir = ir,
                        ValorResgate = resgate
                    });
            }
            foreach (var investimento in rendaFixa.Lcis)
            {
                investimentos.ValorTotal += investimento.CapitalAtual;
                double ir = CalcularIr(investimento.CapitalInvestido, investimento.CapitalAtual, taxaInvestimentoRendaFixa);
                double resgate = investimento.CapitalAtual;
                resgate = CalcularResgateComDesconto(utcNow, investimento.DataOperacao, investimento.Vencimento, resgate);
                investimentos.Investimentos.Add(new Investimento()
                {
                    Nome = investimento.Nome,
                    ValorInvestido = investimento.CapitalInvestido,
                    ValorTotal = investimento.CapitalAtual,
                    Vencimento = investimento.Vencimento,
                    Ir = ir,
                    ValorResgate = resgate
                });
            }
            foreach (var investimento in fundos.Fundos)
            {
                investimentos.ValorTotal += investimento.ValorAtual;
                double ir = CalcularIr(investimento.CapitalInvestido, investimento.ValorAtual, taxaInvestimentoFundos);
                double resgate = investimento.ValorAtual;
                resgate = CalcularResgateComDesconto(utcNow, investimento.DataCompra, investimento.DataResgate, resgate);
                investimentos.Investimentos.Add(new Investimento()
                {
                    Nome = investimento.Nome,
                    ValorInvestido = investimento.CapitalInvestido,
                    ValorTotal = investimento.ValorAtual,
                    Vencimento = investimento.DataResgate,
                    Ir = ir,
                    ValorResgate = resgate
                });
            }
        }

        private static double CalcularIr(double valorInvestido, double valorAtual, double taxaInvestimento)
        {
            return valorAtual - valorInvestido < 0 ? 0 : (valorAtual - valorInvestido) * taxaInvestimento;
        }

        private static double CalcularResgateComDesconto(DateTimeOffset utcNow, DateTimeOffset dataCompra, DateTimeOffset dataVencimento, double resgate)
        {
            if (dataCompra < utcNow.AddMonths(3))
                resgate -= resgate * taxaResgateMaiorQueMeioPeriodo;
            else if (dataVencimento - utcNow < (dataCompra - dataVencimento) / 2)
                resgate -= resgate * taxaResgateAteTresMeses;
            else
                resgate -= resgate * taxaResgateOutros;
            return resgate;
        }
    }
}
