
using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class InvestimentosResponse
    {
        public decimal ValorTotal { get; set; }
        public IList<Investimento> Investimentos { get; set; } = new List<Investimento>();
    }

    public partial class Investimento
    {
        public string Nome { get; set; }
        public decimal ValorInvestido { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Ir { get; set; }
        public decimal ValorResgate { get; set; }
    }
}
