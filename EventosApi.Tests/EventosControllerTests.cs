using Xunit;
using Moq;
using FluentAssertions;
using EventosApi.Services;
using EventosApi.Models;
using EventosApi.Controllers.v1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // Adicionado para o DateTime no novo teste

namespace EventosApi.Tests
{
    public class EventosControllerTests
    {
        private readonly Mock<IEventoService> _mockService;
        private readonly EventosController _controller;

        public EventosControllerTests()
        {
            _mockService = new Mock<IEventoService>();
            _controller = new EventosController(_mockService.Object);
        }

        // Teste 1: Listagem de Eventos
        [Fact]
        public async Task Get_DeveRetornarOk_ComListaDeEventos()
        {
            // --- ARRANGE ---
            var eventosFalsos = GetFakeEventos();
            _mockService.Setup(s => s.GetAsync())
                .ReturnsAsync(eventosFalsos);

            // --- ACT ---
            var actionResult = await _controller.Get();

            // --- ASSERT ---
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result.As<OkObjectResult>();
            okResult.Value.Should().BeAssignableTo<List<Evento>>();
            okResult.Value.As<List<Evento>>().Count.Should().Be(2);
        }

        // Teste 2: Criação de Evento
        [Fact]
        public async Task Post_DeveRetornarCreated_QuandoEventoEValido()
        {
            // --- ARRANGE ---
            var novoEvento = new Evento
            {
                Titulo = "Palestra de DevOps",
                Categoria = "Palestra",
                Data = DateTime.Now.AddDays(30),
                Local = "Online",
                CapacidadeMaxima = 100
            };

            _mockService.Setup(s => s.CreateAsync(It.IsAny<Evento>()))
                .Returns(Task.CompletedTask);

            // --- ACT ---
            var actionResult = await _controller.Post(novoEvento);

            // --- ASSERT ---
            actionResult.Should().BeOfType<CreatedAtRouteResult>();
            var createdResult = actionResult.As<CreatedAtRouteResult>();
            createdResult.RouteName.Should().Be("ObterEvento");
            createdResult.Value.Should().BeEquivalentTo(novoEvento);
            _mockService.Verify(s => s.CreateAsync(novoEvento), Times.Once);
        }

        // --- MÉTODO AUXILIAR ---
        private List<Evento> GetFakeEventos()
        {
            return new List<Evento>
            {
                new Evento { Id = "60c72b2f5f1b2c001c8e4c8a", Titulo = "Show de Rock", Categoria = "Show" },
                new Evento { Id = "60c72b2f5f1b2c001c8e4c8b", Titulo = "Palestra de IA", Categoria = "Palestra" }
            };
        }
    }
}