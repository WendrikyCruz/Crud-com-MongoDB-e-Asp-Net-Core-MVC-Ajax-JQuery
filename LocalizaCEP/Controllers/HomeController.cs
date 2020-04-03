using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LocalizaCEP.Models;
using Newtonsoft.Json;
using Refit;
using LocalizaCEP.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocalizaCEP.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MongoServices _mongoServices;

        [BindProperty]
        public Endereco endereco { get; set; }

        public CepResponse cep = new CepResponse();


        public HomeController(ILogger<HomeController> logger, MongoServices mongoServices)
        {
            _mongoServices = mongoServices;
            _logger = logger;


        }


        public IActionResult Index()
        {
            try
            {
                ViewBag.ListaEndereco = _mongoServices.Get();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View("Index");
        }

        [HttpGet]
        public async Task<string> GetEndereco(string c)
        {
            var cepClient = RestService.For<ICepResponse>("http://viacep.com.br");
            var address = await cepClient.GetAddressAsync(c);

            string output = JsonConvert.SerializeObject(address);
            return output;
        }

        [HttpPost]
        public JsonResult Cadastrar(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                _mongoServices.Create(endereco);
            }

            var listaEnd = JsonConvert.SerializeObject(_mongoServices.Get());
            return new JsonResult(listaEnd);

        }

        [HttpPut]
        public JsonResult EditarEndereco(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                _mongoServices.Update(endereco._Id, endereco);
            }

            var listaEnd = JsonConvert.SerializeObject(_mongoServices.Get());
            return new JsonResult(listaEnd);

        }

        [HttpDelete]
        public JsonResult DeletarEndereco(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                _mongoServices.Remove(endereco._Id);
            }

            var listaEnd = JsonConvert.SerializeObject(_mongoServices.Get());
            return new JsonResult(listaEnd);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
