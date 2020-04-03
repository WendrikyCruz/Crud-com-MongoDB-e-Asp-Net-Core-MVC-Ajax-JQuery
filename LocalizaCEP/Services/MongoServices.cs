using LocalizaCEP.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalizaCEP.Services
{
    public class MongoServices
    {
        private readonly IMongoCollection<Endereco> _Endereco;

        public MongoServices(MongoConfig config)
        {
            var equipamentoColecao = new MongoClient(config.StringConexao);
            var database = equipamentoColecao.GetDatabase(config.NomeBD);

            _Endereco = database.GetCollection<Endereco>("ColecaoEndereco");

        }

        public List<Endereco> Get() =>
           _Endereco.Find(Endereco => true).ToList();

        public Endereco Get(string? id) =>
            _Endereco.Find<Endereco>(Equipamento => Equipamento._Id == id).FirstOrDefault();

        public void Create(Endereco Endereco)
        {
            _Endereco.InsertOne(Endereco);
        }

        public void Update(string idEndereco, Endereco EnderecoIn) =>
            _Endereco.ReplaceOne(Endereco => Endereco._Id == idEndereco, EnderecoIn);

        public void Remove(string EnderecoDelete) => _Endereco.DeleteOne(Endereco => Endereco._Id == EnderecoDelete);
    }
}
