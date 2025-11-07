using EventosApi.ML;
using EventosApi.Services; // (Adicione este se você moveu o serviço para a pasta ML)
using Microsoft.AspNetCore.Mvc;

namespace EventosApi.Controllers
{
    [ApiController]
    [Route("api/previsao")]
    public class PrevisaoController : ControllerBase
    {
        private readonly PrevisaoParticipantesService _previsaoService;
        private readonly ILogger<PrevisaoController> _logger;

        public PrevisaoController(PrevisaoParticipantesService previsaoService, ILogger<PrevisaoController> logger)
        {
            _previsaoService = previsaoService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] PrevisaoRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var input = new EventoPrevisaoInput
            {
                Categoria = request.Categoria,
                CapacidadeMaxima = request.CapacidadeMaxima,
                Mes = request.Mes,
                Participantes = 0
            };

            _logger.LogInformation("Executando previsão para: {Categoria}, {Capacidade}, {Mes}",
                input.Categoria, input.CapacidadeMaxima, input.Mes);

            // 1. O ML nos dá o palpite "matemático" (ex: 588.43)
            EventoPrevisaoOutput previsao = _previsaoService.Predict(input);

            // --- CORREÇÃO DE LÓGICA DE NEGÓCIO ---

            // 2. Travamos o valor máximo na capacidade do evento
            if (previsao.ParticipantesPrevistos > input.CapacidadeMaxima)
            {
                previsao.ParticipantesPrevistos = input.CapacidadeMaxima;
            }

            // 3. Travamos o valor mínimo em 0 (não podemos ter participantes negativos)
            if (previsao.ParticipantesPrevistos < 0)
            {
                previsao.ParticipantesPrevistos = 0;
            }

            // 4. Retornamos o valor corrigido e com "bom senso"
            return Ok(previsao);
        }
    }
}