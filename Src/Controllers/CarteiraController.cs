using EInvest2.Interface;
using EInvest2.Models;
using EInvest2.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EInvest2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarteiraController : ControllerBase
    {
        private const decimal taxaInvestimentoTesouroDireto = 0.1M;
        private const decimal taxaInvestimentoRendaFixa = 0.05M;
        private const decimal taxaInvestimentoFundos = 0.15M;

        private const decimal taxaResgateOutros = 0.3M;
        private const decimal taxaResgateAteTresMeses = 0.15M;
        private const decimal taxaResgateMaiorQueMeioPeriodo = 0.06M;

        private readonly ILogger<CarteiraController> _logger;
        private readonly IMemoryCache _cache;
        private readonly ITesouroDireto _tesouroDiretoService;        
        private readonly IRendaFixa _rendaFixaService;
        private readonly IFundos _fundosService;

        public CarteiraController(ILogger<CarteiraController> logger,
                                  ITesouroDireto tesouroDiretoService,
                                  IRendaFixa rendaFixaService,
                                  IFundos fundosService,
                                  IMemoryCache cache
                                  )
        {
            _tesouroDiretoService = tesouroDiretoService;
            _rendaFixaService = rendaFixaService;
            _fundosService = fundosService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public Task<InvestimentosResponse> Get()
        {
            DateTime dataExpiracao = DateTime.Today.AddDays(1);
            if (!_cache.TryGetValue("InvestimentosResponse", out Task<InvestimentosResponse> investimentosResponse))
            {
                if (investimentosResponse == null)
                {
                    investimentosResponse = Investimentos();
                }
                _cache.Set("InvestimentosResponse", investimentosResponse, dataExpiracao);
            }
            return investimentosResponse;
        }

        private async Task<InvestimentosResponse> Investimentos()
        {
            DateTime dataConsulta = DateTime.Now;
            TesouroDiretoResponse tesouro = await _tesouroDiretoService.Get();
            RendaFixaResponse rendaFixa = await _rendaFixaService.Get();
            FundosResponse fundos = await _fundosService.Get();
            InvestimentosResponse investimentos = new InvestimentosResponse();

            PopulaTsouro(dataConsulta, tesouro, investimentos);
            PopulaRendaFixa(dataConsulta, rendaFixa, investimentos);
            PopulaFundos(dataConsulta, fundos, investimentos);            

            return investimentos;
        }

        private static void PopulaFundos(DateTime dataConsulta, FundosResponse fundos, InvestimentosResponse response)
        {
            foreach (var investimento in fundos.Fundos)
            {
                decimal taxa = VerificaTaxaPeriodo(dataConsulta, investimento.DataCompra, investimento.DataResgate);
                decimal valorResgate = CalcularValorMenosTaxa((investimento.ValorAtual), taxa);
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
        }

        private static void PopulaRendaFixa(DateTime dataConsulta, RendaFixaResponse rendaFixa, InvestimentosResponse response)
        {
            foreach (var investimento in rendaFixa.Lcis)
            {
                decimal taxa = VerificaTaxaPeriodo(dataConsulta, investimento.DataOperacao, investimento.Vencimento);
                decimal valorResgate = CalcularValorMenosTaxa(investimento.CapitalAtual, taxa);
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
        }

        private static void PopulaTsouro(DateTime dataConsulta, TesouroDiretoResponse tesouro, InvestimentosResponse response)
        {
            foreach (var investimento in tesouro.Tds)
            {
                decimal taxa = VerificaTaxaPeriodo(dataConsulta, investimento.DataDeCompra, investimento.Vencimento);
                decimal valorResgate = CalcularValorMenosTaxa(investimento.ValorTotal, taxa);
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
        }

        public static decimal CalcularIr(decimal valorInvestido, decimal valorAtual, decimal taxaInvestimento)
        {
            return InvestimentoDiferencaPositiva(valorInvestido, valorAtual) ? CalcularValorMenosTaxa(valorAtual - valorInvestido, taxaInvestimento) : 0;
        }

        public static bool InvestimentoDiferencaPositiva(decimal valorInvestido, decimal valorAtual) => valorAtual - valorInvestido > 0;

        public static decimal CalcularValorMenosTaxa(decimal valor, decimal taxa) => valor -= valor * taxa;

        public static decimal VerificaTaxaPeriodo(DateTime dataConsulta, DateTime dataCompra, DateTime dataVencimento)
        {
            if ((dataVencimento - dataConsulta < (dataVencimento - dataCompra) / 2) && dataVencimento.AddMonths(-3) > dataConsulta)
                return taxaResgateMaiorQueMeioPeriodo;
            else if (dataVencimento < dataConsulta.AddMonths(3))
                return taxaResgateAteTresMeses;
            else
                return taxaResgateOutros;            
        }
    }
}