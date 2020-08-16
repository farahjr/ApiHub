using EInvest2.Interface;
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
        private readonly ITesouroDireto _tesouroDiretoService;        
        private readonly IRendaFixa _rendaFixaService;
        private readonly IFundos _fundosService;

        public CarteiraController(ILogger<CarteiraController> logger,
                                  ITesouroDireto tesouroDiretoService,
                                  IRendaFixa rendaFixaService,
                                  IFundos fundosService)
        {
            _tesouroDiretoService = tesouroDiretoService;
            _rendaFixaService = rendaFixaService;
            _fundosService = fundosService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<InvestimentosResponse> GetAsync()
        {            
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
            TesouroDiretoResponse tesouro = await _tesouroDiretoService.Get();            
            RendaFixaResponse rendaFixa = await _rendaFixaService.Get();            
            FundosResponse fundos = await _fundosService.Get();            

            InvestimentosResponse investimentos = MontaInvestimentoResponse(utcNow, tesouro, rendaFixa, fundos);
            
            return investimentos;
        }

        private static InvestimentosResponse MontaInvestimentoResponse(DateTimeOffset utcNow, TesouroDiretoResponse tesouro, RendaFixaResponse rendaFixa, FundosResponse fundos)
        {
            InvestimentosResponse response = new InvestimentosResponse();
            foreach (var investimento in tesouro.Tds)
            {
                double taxa = VerificaTaxaPeriodo(utcNow, investimento.DataDeCompra, investimento.Vencimento);
                double valorResgate = CalcularValorMenosTaxa(investimento.ValorTotal, taxa);
                response.ValorTotal += investimento.ValorTotal;                
                response.Investimentos.Add(new Investimento()
                {
                    Nome = investimento.Nome,
                    ValorInvestido = investimento.ValorInvestido,
                    ValorTotal = investimento.ValorTotal,
                    Vencimento = investimento.Vencimento,
                    Ir = CalcularIr(investimento.ValorInvestido, investimento.ValorTotal, taxaInvestimentoTesouroDireto),
                    ValorResgate = valorResgate
                });
            }
            foreach (var investimento in rendaFixa.Lcis)
            {
                double taxa = VerificaTaxaPeriodo(utcNow, investimento.DataOperacao, investimento.Vencimento);
                double valorResgate = CalcularValorMenosTaxa(investimento.CapitalAtual, taxa);
                response.ValorTotal += investimento.CapitalAtual;
                response.Investimentos.Add(new Investimento()
                {
                    Nome = investimento.Nome,
                    ValorInvestido = investimento.CapitalInvestido,
                    ValorTotal = investimento.CapitalAtual,
                    Vencimento = investimento.Vencimento,
                    Ir = CalcularIr(investimento.CapitalInvestido, investimento.CapitalAtual, taxaInvestimentoRendaFixa),
                    ValorResgate = valorResgate
                });
            }
            foreach (var investimento in fundos.Fundos)
            {
                double taxa = VerificaTaxaPeriodo(utcNow, investimento.DataCompra, investimento.DataResgate);
                double valorResgate = CalcularValorMenosTaxa((investimento.ValorAtual), taxa);
                response.ValorTotal += investimento.ValorAtual;                
                response.Investimentos.Add(new Investimento()
                {
                    Nome = investimento.Nome,
                    ValorInvestido = investimento.CapitalInvestido,
                    ValorTotal = investimento.ValorAtual,
                    Vencimento = investimento.DataResgate,
                    Ir = CalcularIr(investimento.CapitalInvestido, investimento.ValorAtual, taxaInvestimentoFundos),
                    ValorResgate = valorResgate
                });
            }
            return response;
        }

        private static double CalcularIr(double valorInvestido, double valorAtual, double taxaInvestimento)
        {
            return InvestimentoDiferencaPositiva(valorInvestido, valorAtual) ? CalcularValorMenosTaxa(valorAtual - valorInvestido, taxaInvestimento) : 0;
        }

        private static bool InvestimentoDiferencaPositiva(double valorInvestido, double valorAtual) => valorAtual - valorInvestido > 0;

        private static double CalcularValorMenosTaxa(double valor, double taxa) => valor -= valor * taxa;

        private static double VerificaTaxaPeriodo(DateTimeOffset utcNow, DateTimeOffset dataCompra, DateTimeOffset dataVencimento)
        {
            if (dataCompra < utcNow.AddMonths(3))
                return taxaResgateMaiorQueMeioPeriodo;
            else if (dataVencimento - utcNow < (dataCompra - dataVencimento) / 2)
                return taxaResgateAteTresMeses;
            else
                return taxaResgateOutros;            
        }
    }
}
