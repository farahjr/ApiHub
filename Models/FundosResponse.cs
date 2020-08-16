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
        [JsonProperty("capitalInvestido")]
        public long CapitalInvestido { get; set; }

        [JsonProperty("ValorAtual")]
        public double ValorAtual { get; set; }

        [JsonProperty("dataResgate")]
        public DateTimeOffset DataResgate { get; set; }

        [JsonProperty("dataCompra")]
        public DateTimeOffset DataCompra { get; set; }

        [JsonProperty("iof")]
        public long Iof { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("totalTaxas")]
        public double TotalTaxas { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
