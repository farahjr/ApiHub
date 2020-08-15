using EInvest2.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EInvest2
{
    public static class Bootstrapper
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddHttpClient<ITesouroDireto, TesouroDiretoService>();
            //services.AddHttpClient<ITesouroDireto, TesouroDiretoService>(client => client.BaseAddress = new Uri("http://www.mocky.io/v2/5e3428203000006b00d9632a"));
            //services.AddHttpClient<IRendaFixa, RendaFixaService>();
            //services.AddHttpClient<ITesouroDireto, TesouroDiretoService>();
        }
    }
}