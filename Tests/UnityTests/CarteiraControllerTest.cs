using System;
using Xunit;
using EInvest2.Controllers;

namespace UnityTests
{

    public class CarteiraControllerTest
    {
        private const decimal taxaResgateOutros = 0.3M;
        private const decimal taxaResgateAteTresMeses = 0.15M;
        private const decimal taxaResgateMaiorQueMeioPeriodo = 0.06M;

        [Theory]
        [InlineData(1,0.5,0.5)]
        [InlineData(100.5, 0.5, 50.25)]
        [InlineData(5, 0.5, 2.5)]
        [InlineData(0.5, 0.5, 0.25)]
        public void CalcularValorMenosTaxa_DeveCalcularCorretamente(decimal valor, decimal taxa, decimal expected)
        {
            decimal actual = CarteiraController.CalcularValorMenosTaxa(valor, taxa);

            Assert.Equal(expected,actual);
        }

        [Fact]        
        public void VerificaTaxaPeriodo_DeveRetornarMaiorQueMeioPeriodo()
        {            
            DateTime dataCompra = DateTime.Parse("2020-01-01T00:00:00");
            DateTime dataConsulta = dataCompra.AddMonths(12);
            DateTime dataVencimento = DateTime.Parse("2021-06-01T00:00:00");
            decimal expected = taxaResgateMaiorQueMeioPeriodo;
            decimal actual = CarteiraController.VerificaTaxaPeriodo(dataConsulta, dataCompra, dataVencimento);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerificaTaxaPeriodo_DeveRetornarAteTresMeses()
        {            
            DateTime dataCompra = DateTime.Parse("2020-01-01T00:00:00");
            DateTime dataVencimento = DateTime.Parse("2021-01-01T00:00:00");
            DateTime dataConsulta = dataCompra.AddMonths(11);
            decimal expected = taxaResgateAteTresMeses;
            decimal actual = CarteiraController.VerificaTaxaPeriodo(dataConsulta, dataCompra, dataVencimento);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VerificaTaxaPeriodo_DeveRetornarTaxaResgateOutros()
        {
            DateTime dataCompra = DateTime.Parse("2020-01-01T00:00:00");
            DateTime dataVencimento = DateTime.Parse("2021-01-01T00:00:00");
            DateTime dataConsulta = dataCompra.AddMonths(1);
            decimal expected = taxaResgateOutros;
            decimal actual = CarteiraController.VerificaTaxaPeriodo(dataConsulta, dataCompra, dataVencimento);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(100, 110, false)]
        [InlineData(1, 1.5, false)]
        [InlineData(500, 0.1, true)]
        [InlineData(500000000.5689, 400000000.5689, true)]
        public void InvestimentoDiferencaPositiva_DeveCalcularCorretamente(decimal valorAtual, decimal valor, bool expected)
        {
            bool actual = CarteiraController.InvestimentoDiferencaPositiva(valor, valorAtual);

            Assert.Equal(expected, actual);
        }
    }
}
