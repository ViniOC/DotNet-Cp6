namespace EventosApi.ML
{
    /// <summary>
    /// Representa o JSON enviado para o endpoint /api/previsao
    /// </summary>
    public class PrevisaoRequestDto
    {
        // Usamos float porque o ML.NET trabalha melhor com float
        public float Mes { get; set; }
        public float CapacidadeMaxima { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}