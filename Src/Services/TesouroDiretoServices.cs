using EInvest2.Interface;
using EInvest2.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EInvest2.Service
{
    public class TesouroDiretoService : ITesouroDireto
    {
        private readonly ILogger<TesouroDiretoService> _logger;
        private const string BaseUrl = "http://www.mocky.io/v2/5e3428203000006b00d9632a";
        private readonly HttpClient _client = new HttpClient();

        public TesouroDiretoService(HttpClient http, ILogger<TesouroDiretoService> logger)
        {
            _logger = logger;
            _client = http;
        }

        public async Task<TesouroDiretoResponse> Get()
        {
            _logger.LogInformation("Buscando Tesouro Direto");
            var httpResponse = await _client.GetAsync(_client.BaseAddress);

            if (!httpResponse.IsSuccessStatusCode)            
                throw new Exception("Problema ao consultar Tesouro Direto");
            
            var response = await httpResponse.Content.ReadAsStringAsync();
            var tesouroDireto = JsonConvert.DeserializeObject<TesouroDiretoResponse>(response);
            _logger.LogInformation("Retornou Tesouro Direto");
            return tesouroDireto;
        }
    }
}
