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
        private readonly HttpClient _client = new HttpClient();

        public FundosService(HttpClient http, ILogger<FundosService> logger)
        {
            _logger = logger;
            _client = http;
        }
        public async Task<FundosResponse> Get()
        {
            _logger.LogInformation("Buscando Fundos");
            var httpResponse = await _client.GetAsync(_client.BaseAddress);

            if (!httpResponse.IsSuccessStatusCode)            
                _logger.LogInformation("Buscando Fundos");            

            var response = await httpResponse.Content.ReadAsStringAsync();
            var fundos = JsonConvert.DeserializeObject<FundosResponse>(response);
            _logger.LogInformation("Retornou Fundos com Sucesso");
            return fundos;
        }
    }
}
