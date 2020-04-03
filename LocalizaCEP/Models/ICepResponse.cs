using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalizaCEP.Models
{
    public interface ICepResponse
    {
        [Get("/ws/{cep}/json/")]
        Task<CepResponse> GetAddressAsync(String cep);
    }
}
