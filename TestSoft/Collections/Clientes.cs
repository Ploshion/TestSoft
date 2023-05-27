using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NSwag.Annotations;
using Swashbuckle.AspNetCore; // Asegúrate de tener esta referencia


namespace TestSoft.Collections
{
    public class Clientes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Obsolete]
        public string? _id { get; set; }

        public string? nombre { get; set; }

        public string? apellido { get; set; }

        public int ? identificacion { get; set;}

        public DateTime? fechaNacimiento { get; set; }

        public string? direccion { get; set; }

        public string? ciudad {get; set;}


    }
}
