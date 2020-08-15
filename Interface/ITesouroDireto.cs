using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EInvest2.Models;
using Newtonsoft.Json;

namespace EInvest2.Service
{
    public interface ITesouroDireto
    {
        Task<TesouroDiretoModel> Get();
    }
}

