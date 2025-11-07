using EventosApi.Models;
using EventosApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventosApi.Services
{
    public class EventoService : IEventoService
    {
        private readonly IMongoCollection<Evento> _eventosCollection;

        public EventoService(IOptions<EventosDatabaseSettings> settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _eventosCollection = database.GetCollection<Evento>(settings.Value.CollectionName);
        }

        /// <summary>
        /// Lista todos os eventos cadastrados.
        /// </summary>
        public async Task<List<Evento>> GetAsync() =>
            await _eventosCollection.Find(_ => true).ToListAsync();

        /// <summary>
        /// Retorna os detalhes de um evento específico pelo ID.
        /// </summary>
        public async Task<Evento?> GetAsync(string id) =>
            await _eventosCollection.Find(e => e.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Cria um novo evento cultural.
        /// </summary>
        public async Task CreateAsync(Evento novoEvento)
        {
            // Garante que a data de criação seja definida no momento da inserção
            novoEvento.DataCriacao = DateTime.UtcNow;
            await _eventosCollection.InsertOneAsync(novoEvento);
        }

        /// <summary>
        /// Atualiza os dados de um evento cultural existente.
        /// </summary>
        public async Task UpdateAsync(string id, Evento eventoAtualizado)
        {
            // O controlador já validou se o evento existe.
            await _eventosCollection.ReplaceOneAsync(e => e.Id == id, eventoAtualizado);
        }

        /// <summary>
        /// Exclui um evento cultural baseado no ID.
        /// </summary>
        public async Task RemoveAsync(string id)
        {
            // O controlador já validou se o evento existe.
            await _eventosCollection.DeleteOneAsync(e => e.Id == id);
        }
    }
}