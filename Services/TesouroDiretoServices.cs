﻿using EInvest2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EInvest2.Service
{
    public class TesouroDiretoService : ITesouroDireto
    {
        private const string BaseUrl = "http://www.mocky.io/v2/5e3428203000006b00d9632a";
        private readonly HttpClient _client = new HttpClient();

        public async Task<TesouroDiretoModel> Get()
        {
            var httpResponse = await _client.GetAsync(BaseUrl);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var todoItem = JsonConvert.DeserializeObject<TesouroDiretoModel>(content);

            return todoItem;
        }
    }
}