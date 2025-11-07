using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventosApi.Models
{
    public class Evento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public string Local { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public int CapacidadeMaxima { get; set; }

        public DateTime DataCriacao { get; set; }

        // --- NOVO CAMPO ADICIONADO (CP06 - Parte 2) ---
        public string? Organizador { get; set; } // 

        public Evento()
        {
            DataCriacao = DateTime.UtcNow;
        }
    }
}