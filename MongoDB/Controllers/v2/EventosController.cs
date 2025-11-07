using Asp.Versioning;
using EventosApi.Models;
using EventosApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventosApi.Controllers.v2
{
    [ApiController]

    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/eventos")]
    public class EventosController : ControllerBase
    {

        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get()
        {
            var eventos = await _eventoService.GetAsync();
            return Ok(eventos);
        }

        [HttpGet("{id:length(24)}", Name = "ObterEventoV2")]
        public async Task<ActionResult<Evento>> Get(string id)
        {
            var evento = await _eventoService.GetAsync(id);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento novoEvento)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _eventoService.CreateAsync(novoEvento);

            // Usamos o novo nome da rota v2
            return CreatedAtRoute("ObterEventoV2", new { id = novoEvento.Id }, novoEvento);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Evento eventoAtualizado)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var eventoExistente = await _eventoService.GetAsync(id);
            if (eventoExistente == null) return NotFound();

            eventoAtualizado.Id = id;
            // Se o JSON do PUT tiver o campo "Organizador", 
            // ele será salvo aqui automaticamente.
            await _eventoService.UpdateAsync(id, eventoAtualizado);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var evento = await _eventoService.GetAsync(id);
            if (evento == null) return NotFound();

            await _eventoService.RemoveAsync(id);
            return NoContent();
        }
    }
}