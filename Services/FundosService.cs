using EInvest2.Interface;
using EInvest2.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EInvest2.Service
{
    public class FundosService : IFundos
    {
        private readonly ILogger<FundosService> _logger;
        private const string BaseUrl = "http://www.mocky.io/v2/5e342ab33000008c00d96342";
        private readonly HttpClient _client = new HttpClient();

        public FundosService(HttpClient http, ILogger<FundosService> logger)
        {
            _logger = logger;
            _client = http;
        }
        public async Task<FundosResponse> Get()
        {
            _logger.LogInformation("Buscando Fundos");
            var httpResponse = await _client.GetAsync($"{BaseUrl}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("Buscando Fundos");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var todoItem = JsonConvert.DeserializeObject<FundosResponse>(content);
            _logger.LogInformation("Retornou Fundos com Sucesso");
            return todoItem;
        }
    }
}
