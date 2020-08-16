
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

namespace EInvest2.Models
{
    public partial class InvestimentosResponse
    {
        public InvestimentosResponse(List<Investimento> list)
        {
            Investimentos = list;
        }

        public double ValorTotal { get; set; }
        public List<Investimento> Investimentos { get; set; }
    }

    public partial class Investimento
    {
        public Investimento(string nome, double valorInvestido, double valorTotal, DateTimeOffset vencimento, double ir, double valorResgate)
        {
            Nome = nome;
            ValorInvestido = valorInvestido;
            ValorTotal = valorTotal;
            Vencimento = vencimento;
            Ir = ir;
            ValorResgate = valorResgate;
        }

        public string Nome { get; set; }
        public double ValorInvestido { get; set; }
        public double ValorTotal { get; set; }
        public DateTimeOffset Vencimento { get; set; }
        public double Ir { get; set; }
        public double ValorResgate { get; set; }
    }

    //public partial class InvestimentosResponse
    //{
    //    public static InvestimentosResponse FromJson(string json) => JsonConvert.DeserializeObject<InvestimentosResponse>(json, EInvest2.Service.Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this InvestimentosResponse self) => JsonConvert.SerializeObject(self, EInvest2.Service.Converter.Settings);
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
