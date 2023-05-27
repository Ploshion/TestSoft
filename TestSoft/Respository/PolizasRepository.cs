using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using TestSoft.Collections;

namespace TestSoft.Respository
{
    public class PolizasRepository: IPolizasRepository
    {

        private readonly IMongoCollection<Polizas> _mongoPolizasCollection;

        private readonly IMongoCollection<Clientes> _mongoClientesCollection;
        public PolizasRepository(IMongoDatabase mongoDatabase) 
        {

            _mongoPolizasCollection = mongoDatabase.GetCollection<Polizas>("Polizas");
            _mongoClientesCollection = mongoDatabase.GetCollection<Clientes>("Clientes");

        }

        public async Task<List<Polizas>> GetAllAsync()
        {
            return await _mongoPolizasCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Polizas>> GetAsync()
        {

           return await _mongoPolizasCollection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
