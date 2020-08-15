using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class FundosResponse
    {
        [JsonProperty("fundos")]
        public List<Fundo> Fundos { get; set; }
    }

    public partial class Fundo
    {
        public double ValorInvestido { get; set; }
        public double ValorTotal { get; set; }
        public DateTimeOffset Vencimento { get; set; }
        public DateTimeOffset DataDeCompra { get; set; }
        public long Iof { get; set; }
        public string Indice { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
    }

}
