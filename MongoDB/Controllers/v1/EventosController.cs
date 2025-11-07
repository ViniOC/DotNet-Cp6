using Asp.Versioning; // Requer o pacote NuGet Asp.Versioning.Mvc.ApiExplorer
using EventosApi.Models;
using EventosApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventosApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")] // Define a versão deste controlador
    [Route("api/v{version:apiVersion}/eventos")] // Rota baseada na versão
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // GET: api/v1/eventos
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get()
        {
            var eventos = await _eventoService.GetAsync();
            return Ok(eventos); // Retorna 200 OK
        }

        // GET: api/v1/eventos/{id}
        [HttpGet("{id:length(24)}", Name = "ObterEvento")]
        public async Task<ActionResult<Evento>> Get(string id)
        {
            var evento = await _eventoService.GetAsync(id);

            if (evento == null)
            {
                return NotFound(); // Retorna 404 Not Found
            }

            return Ok(evento); // Retorna 200 OK
        }

        // POST: api/v1/eventos
        [HttpPost]
        public async Task<IActionResult> Post(Evento novoEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna 400 Bad Request
            }

            await _eventoService.CreateAsync(novoEvento);

            // Retorna 201 Created, com o link para o novo recurso
            return CreatedAtRoute("ObterEvento", new { id = novoEvento.Id }, novoEvento);
        }

        // PUT: api/v1/eventos/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Evento eventoAtualizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna 400 Bad Request
            }

            var eventoExistente = await _eventoService.GetAsync(id);
            if (eventoExistente == null)
            {
                return NotFound(); // Retorna 404 Not Found
            }

            eventoAtualizado.Id = id;
            await _eventoService.UpdateAsync(id, eventoAtualizado);

            return NoContent(); // Retorna 204 No Content
        }

        // DELETE: api/v1/eventos/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var evento = await _eventoService.GetAsync(id);
            if (evento == null)
            {
                return NotFound(); // Retorna 404 Not Found
            }

            await _eventoService.RemoveAsync(id);

            return NoContent(); // Retorna 204 No Content
        }
    }
}