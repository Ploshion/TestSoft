using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TestSoft.Collections
{
    public class CreacionPoliza
    {
        [BsonElement("clienteData")]
        public List<Clientes>? clienteData { get; set; }

        [BsonElement("clienteId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? clienteId { get; set; }

        [BsonElement("coberturaId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? coberturaId { get; set; }

        [BsonElement("fechaTomaPoliza")]
        public DateTime fechaTomaPoliza { get; set; }

        [BsonElement("vlrMax")]
        public float vlrMax { get; set; }

        [BsonElement("nombrePlan")]
        public string? nombrePlan { get; set; }

        [BsonElement("placaAutomotor")]
        public string? placaAutomotor { get; set; }

        [BsonElement("modelo")]
        public int modelo { get; set; }

        [BsonElement("inspeccionVehi")]
        public bool inspeccionVehi { get; set; }

    }
}
