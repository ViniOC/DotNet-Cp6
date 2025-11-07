using EventosApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventosApi.Services
{
    // Usar uma interface é uma boa prática para facilitar os testes (CP06) e a injeção de dependência
    public interface IEventoService
    {
        Task<List<Evento>> GetAsync();
        Task<Evento?> GetAsync(string id);
        Task CreateAsync(Evento novoEvento);
        Task UpdateAsync(string id, Evento eventoAtualizado);
        Task RemoveAsync(string id);
    }
}