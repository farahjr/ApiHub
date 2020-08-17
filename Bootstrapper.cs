using EInvest2.Interface;
using EInvest2.Service;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EInvest2
{
    public static class Bootstrapper
    {
        public static void UseServices(this IServiceCollection services)
        {            
            services.AddHttpClient<ITesouroDireto, TesouroDiretoService>(client => client.BaseAddress = new Uri("http://www.mocky.io/v2/5e3428203000006b00d9632a"));
            services.AddHttpClient<IRendaFixa, RendaFixaService>(client => client.BaseAddress = new Uri("http://www.mocky.io/v2/5e3429a33000008c00d96336"));
            services.AddHttpClient<IFundos, FundosService>(client => client.BaseAddress = new Uri("http://www.mocky.io/v2/5e342ab33000008c00d96342"));
        }
    }
}