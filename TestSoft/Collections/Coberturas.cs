using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TestSoft.Collections
{
    public class Coberturas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
         
        public string? _id {get; set;}

        [BsonElement("nombre")]
        public string? nombre { get; set;}

        [BsonElement("descripcion")]
        public string? descripcion { get; set; }

        [BsonElement("ValorMaximo")]
        public int? ValorMaximo { get; set; }


    }
}
