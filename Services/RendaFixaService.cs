using EInvest2.Interface;
using EInvest2.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EInvest2.Service
{
    public class RendaFixaService : IRendaFixa
    {
        private readonly ILogger<RendaFixaService> _logger;        
        private readonly HttpClient _client = new HttpClient();

        public RendaFixaService(HttpClient http, ILogger<RendaFixaService> logger)
        {
            _logger = logger;
            _client = http;
        }
        public async Task<RendaFixaResponse> Get()
        {
            _logger.LogInformation("Buscando RendaFixa");
            var httpResponse = await _client.GetAsync(_client.BaseAddress);

            if (!httpResponse.IsSuccessStatusCode)            
                throw new Exception("Problema ao consultar RendaFixa");            

            var response = await httpResponse.Content.ReadAsStringAsync();
            var rendaFixa = JsonConvert.DeserializeObject<RendaFixaResponse>(response);
            _logger.LogInformation("Retornou RendaFixa");
            return rendaFixa;
        }
    }
}
