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
        private const string BaseUrl = "http://www.mocky.io/v2/5e3429a33000008c00d96336";
        private readonly HttpClient _client = new HttpClient();

        public RendaFixaService(HttpClient http, ILogger<RendaFixaService> logger)
        {
            _logger = logger;
            _client = http;
        }

        public async Task<RendaFixaResponse> Get()
        {
            _logger.LogInformation("Buscando RendaFixa");
            var httpResponse = await _client.GetAsync($"{BaseUrl}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Problema ao consultar RendaFixa");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var todoItem = JsonConvert.DeserializeObject<RendaFixaResponse>(content);
            _logger.LogInformation("Retornou RendaFixa");
            return todoItem;
        }
    }
}
