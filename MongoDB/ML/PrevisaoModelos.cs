using Microsoft.ML.Data;

namespace EventosApi.ML
{
    /// <summary>
    /// Classe de ENTRADA do modelo.
    /// Define os dados que usamos para treinar (Features).
    /// </summary>
    public class EventoPrevisaoInput
    {
        // O ML.NET usa "float" para a maioria dos números

        [LoadColumn(0)]
        public float Mes { get; set; }

        [LoadColumn(1)]
        public float CapacidadeMaxima { get; set; }

        [LoadColumn(2)]
        public string Categoria { get; set; } = string.Empty;

        // Esta é a coluna que queremos prever (o "Label")
        [LoadColumn(3)]
        public float Participantes { get; set; }
    }

    /// <summary>
    /// Classe de SAÍDA do modelo.
    /// Define o que o modelo irá retornar.
    /// </summary>
    public class EventoPrevisaoOutput
    {
        // O [ColumnName("Score")] é obrigatório.
        // É o nome padrão do ML.NET para o resultado da previsão.
        [ColumnName("Score")]
        public float ParticipantesPrevistos { get; set; }
    }
}