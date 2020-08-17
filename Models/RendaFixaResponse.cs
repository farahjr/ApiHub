using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class RendaFixaResponse
    {
        public List<Lci> Lcis { get; set; }
    }

    public partial class Lci
    {
        public decimal CapitalInvestido { get; set; }
        public decimal CapitalAtual { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Iof { get; set; }
        public decimal OutrasTaxas { get; set; }
        public decimal Taxas { get; set; }
        public string Indice { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public bool GuarantidoFgc { get; set; }
        public DateTime DataOperacao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public bool Primario { get; set; }
    }
}
