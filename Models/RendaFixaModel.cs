using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class RendaFixaModel
    {
        public List<Lci> Lcis { get; set; }
    }

    public partial class Lci
    {
        public long CapitalInvestido { get; set; }
        public double CapitalAtual { get; set; }
        public long Quantidade { get; set; }
        public DateTimeOffset Vencimento { get; set; }
        public long Iof { get; set; }
        public long OutrasTaxas { get; set; }
        public long Taxas { get; set; }
        public string Indice { get; set; }
        public string Tipo { get; set; } 
        public string Nome { get; set; }        
        public bool GuarantidoFgc { get; set; }        
        public DateTimeOffset DataOperacao { get; set; }        
        public double PrecoUnitario { get; set; }        
        public bool Primario { get; set; }
    }

    //public partial class RendaFixaModel
    //{
    //    public static RendaFixaModel FromJson(string json) => JsonConvert.DeserializeObject<RendaFixaModel>(json, EInvest2.Service.Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this RendaFixaModel self) => JsonConvert.SerializeObject(self, EInvest2.Service.Converter.Settings);
    //}

    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}
