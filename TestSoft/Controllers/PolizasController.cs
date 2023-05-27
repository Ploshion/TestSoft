using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using TestSoft.Collections;
using TestSoft.Respository;

namespace TestSoft.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PolizasController : ControllerBase
    {

        private readonly IPolizasRepository _polizasRepository;

        private readonly IMongoCollection<Polizas> _mongoPolizasCollection;

        private readonly IMongoCollection<Clientes> _mongoClientesCollection;

        private readonly IMongoCollection<Coberturas> _mongoCoberturasCollection;


        public PolizasController(IPolizasRepository polizasRepository, IMongoDatabase mongoDatabase)
        {
            _polizasRepository = polizasRepository;


            _mongoPolizasCollection = mongoDatabase.GetCollection<Polizas>("Polizas");
            _mongoClientesCollection = mongoDatabase.GetCollection<Clientes>("Clientes");
            _mongoCoberturasCollection = mongoDatabase.GetCollection<Coberturas>("Coberturas");

        }

        [HttpGet]
        
        public async Task<IActionResult> Get()
        {
            var poliza = await _polizasRepository.GetAllAsync();
            return Ok(poliza);
        }

        [HttpGet("Clientes")]

        public async Task<List<Clientes>> GetClientes()
        {
            return await _mongoClientesCollection.Find(new BsonDocument()).ToListAsync();
        }



        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Polizas>>> GetPolizas()
        {
            var pipeline = new BsonDocument[]
            {
               // Etapa 1: Realizar la operación de join con la colección de clientes
                new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "Clientes" },
                        { "let", new BsonDocument("clienteId", "$clienteId") },
                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument("$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$clienteId" })))
                            }
                        },
                        { "as", "clienteData" }
                    }),

                new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "Coberturas" },
                        { "let", new BsonDocument("coberturaId", "$coberturaId") },
                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument("$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$coberturaId" })))
                            }
                        },
                        { "as", "coberturaData" }
                    }),

               // Etapa 2: Proyectar solo los campos necesarios
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "clienteId", 1 },
                    { "coberturaId", 1 },
                    { "fechaTomaPoliza", 1 },
                    { "vlrMax", 1 },
                    { "nombrePlan", 1 },
                    { "placaAutomotor", 1 },
                    { "modelo", 1 },
                    { "inspeccionVehi", 1 },
                    { "clienteData", "$clienteData" },
                    { "coberturaData", 1 }
                })
       };

            var polizas = await _mongoPolizasCollection.Aggregate<Polizas>(pipeline).ToListAsync();
            return polizas;
        }

        public class AgregarDatosRequest
        {
            public CreacionPoliza CreacionPoliza { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> AgregarDatos([FromBody] AgregarDatosRequest request)
        {
            var creacionPoliza = request.CreacionPoliza;

            // Validar la fechaTomaPoliza
            if (creacionPoliza.fechaTomaPoliza < DateTime.Now)
            {
                return BadRequest("La fecha de toma de poliza no puede ser menor a la fecha actual.");
            }

            // Obtener el objeto cliente de la lista de clienteData
            var cliente = creacionPoliza.clienteData[0];

            // Agregar cliente a la colección de clientes
            _mongoClientesCollection.InsertOne(cliente);

            // Obtener el ID del cliente creado
            var clienteId = cliente._id;



            // Crear un objeto Polizas utilizando los datos de creacionPoliza
            var poliza = new Polizas
            {
                clienteId = clienteId,
                coberturaId = creacionPoliza.coberturaId,
                fechaTomaPoliza = creacionPoliza.fechaTomaPoliza,
                vlrMax = creacionPoliza.vlrMax,
                nombrePlan = creacionPoliza.nombrePlan,
                placaAutomotor = creacionPoliza.placaAutomotor,
                modelo = creacionPoliza.modelo,
                inspeccionVehi = creacionPoliza.inspeccionVehi,
                fechaVigencia = creacionPoliza.fechaTomaPoliza.AddYears(1)
        };

            // Agregar poliza a la colección de polizas
            _mongoPolizasCollection.InsertOne(poliza);

            // Devolver un resultado exitoso
            return Ok(); // Puedes personalizar el resultado si lo deseas
        }


        [HttpGet("{parametro}")]
        public async Task<IActionResult> ObtenerPorPlacaOId(string parametro)
        {

            BsonDocument matchFilter;

            if (ObjectId.TryParse(parametro, out var objectId))
            {
                // El parámetro es un ObjectId válido
                matchFilter = new BsonDocument("_id", objectId);
            }
            else
            {
                // El parámetro no es un ObjectId válido, asumimos que es una placaAutomotor
                matchFilter = new BsonDocument("placaAutomotor", parametro);
            }

            var pipeline = new BsonDocument[]
            {
                // Etapa 1: Realizar la operación de join con la colección de clientes
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Clientes" },
                    { "let", new BsonDocument("clienteId", "$clienteId") },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument("$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$clienteId" })))
                        }
                    },
                    { "as", "clienteData" }
                }),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Coberturas" },
                    { "let", new BsonDocument("coberturaId", "$coberturaId") },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument("$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$coberturaId" })))
                        }
                    },
                    { "as", "coberturaData" }
                }),

                // Etapa 2: Proyectar solo los campos necesarios
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "clienteId", 1 },
                    { "coberturaId", 1 },
                    { "fechaTomaPoliza", 1 },
                    { "vlrMax", 1 },
                    { "nombrePlan", 1 },
                    { "placaAutomotor", 1 },
                    { "modelo", 1 },
                    { "inspeccionVehi", 1 },
                    { "clienteData", "$clienteData" },
                    { "coberturaData", 1 }
                }),

                // Etapa 3: Filtrar por parámetro adicional (placaAutomotor o _id)
                new BsonDocument("$match", matchFilter)

            };

            var polizas = await _mongoPolizasCollection.Aggregate<Polizas>(pipeline).ToListAsync();
            return Ok(polizas);
        }





    }

}

