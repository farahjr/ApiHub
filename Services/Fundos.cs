using EInvest2.Interface;
using EInvest2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EInvest2.Service
{
    public class FundosService : IFundos
    {
        private const string BaseUrl = "http://www.mocky.io/v2/5e342ab33000008c00d96342";
        private readonly HttpClient _client = new HttpClient();

        public async Task<FundosResponse> Get()
        {
            var httpResponse = await _client.GetAsync($"{BaseUrl}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var todoItem = JsonConvert.DeserializeObject<FundosResponse>(content);

            return todoItem;
        }
    }
}
