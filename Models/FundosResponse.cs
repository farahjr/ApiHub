using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class FundosResponse
    {
        public List<Fundo> Fundos { get; set; }
    }

    public partial class Fundo
    {        
        public decimal CapitalInvestido { get; set; }
        public decimal ValorAtual { get; set; }
        public DateTime DataResgate { get; set; }        
        public DateTime DataCompra { get; set; }
        public decimal Iof { get; set; }
        public string Nome { get; set; }
        public decimal TotalTaxas { get; set; }
        public int Quantity { get; set; }
    }
}
