using Cadastro_Pessoa.Controllers;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CadastroPessoa.Tests.Controllers
{
    public class PessoaIntegrationTests
    {
        private readonly Mock<IPessoaService> _mockService;
        private readonly PessoaController _controller;

        public PessoaIntegrationTests()
        {
            _mockService = new Mock<IPessoaService>();
            _controller = new PessoaController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_RetornaOkComLista()
        {
            // Arrange
            var pessoas = new List<Pessoa>
            {
                new Pessoa { Id = 1, Nome = "João Silva" },
                new Pessoa { Id = 2, Nome = "Maria Oliveira" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(pessoas);

            // Act
            var resultado = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var lista = Assert.IsAssignableFrom<IEnumerable<Pessoa>>(okResult.Value);
            Assert.Equal(2, lista.Count());
        }

        [Fact]
        public async Task GetById_RetornaOkComPessoa()
        {
            // Arrange
            var pessoa = new Pessoa { Id = 1, Nome = "João Silva" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(pessoa);

            // Act
            var resultado = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var pessoaRetornada = Assert.IsType<Pessoa>(okResult.Value);
            Assert.Equal("João Silva", pessoaRetornada.Nome);
        }

        [Fact]
        public async Task Create_RetornaCreatedAtAction()
        {
            // Arrange
            var novaPessoa = new Pessoa { Id = 3, Nome = "Ana Souza" };
            _mockService.Setup(s => s.CreateAsync(novaPessoa)).ReturnsAsync(novaPessoa);

            // Act
            var resultado = await _controller.Create(novaPessoa);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var pessoaCriada = Assert.IsType<Pessoa>(createdResult.Value);
            Assert.Equal("Ana Souza", pessoaCriada.Nome);
            Assert.Equal(3, pessoaCriada.Id);
        }
    }
}
