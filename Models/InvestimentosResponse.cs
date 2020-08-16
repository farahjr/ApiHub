
using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class InvestimentosResponse
    {
        //public InvestimentosResponse(List<Investimento> list)
        //{
        //    Investimentos = list;
        //}

        public double ValorTotal { get; set; }
        public IList<Investimento> Investimentos { get; set; } = new List<Investimento>();
    }

    public partial class Investimento
    {
        //public Investimento(string nome, double valorInvestido, double valorTotal, DateTimeOffset vencimento, double ir, double valorResgate)
        //{
        //    Nome = nome;
        //    ValorInvestido = valorInvestido;
        //    ValorTotal = valorTotal;
        //    Vencimento = vencimento;
        //    Ir = ir;
        //    ValorResgate = valorResgate;
        //}

        public string Nome { get; set; }
        public double ValorInvestido { get; set; }
        public double ValorTotal { get; set; }
        public DateTimeOffset Vencimento { get; set; }
        public double Ir { get; set; }
        public double ValorResgate { get; set; }
    }
}
