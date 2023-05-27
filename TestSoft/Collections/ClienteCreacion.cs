using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TestSoft.Collections
{
    public class ClienteCreacion
    {
      
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            [Obsolete]
            public string? _id { get; set; }

            public string? nombre { get; set; }

            public string? apellido { get; set; }

            public int? identificacion { get; set; }

            public DateTime? fechaNacimiento { get; set; }

            public string? direccion { get; set; }

            public string? ciudad { get; set; }


        
    }
}
