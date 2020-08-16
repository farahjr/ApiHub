using System;
using System.Collections.Generic;

namespace EInvest2.Models
{
    public partial class TesouroDiretoResponse
    {

        public List<TesouroDireto> Tds { get; set; }
    }

    public partial class TesouroDireto
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
