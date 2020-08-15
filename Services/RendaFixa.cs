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
    public class RendaFixaService : IRendaFixa
    {
        private const string BaseUrl = "http://www.mocky.io/v2/5e3429a33000008c00d96336";
        private readonly HttpClient _client = new HttpClient();

        public async Task<RendaFixaModel> Get()
        {
            var httpResponse = await _client.GetAsync($"{BaseUrl}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var todoItem = JsonConvert.DeserializeObject<RendaFixaModel>(content);

            return todoItem;
        }
    }
}
