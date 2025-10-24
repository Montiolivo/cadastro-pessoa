using Cadastro_Pessoa.Controllers;
using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CadastroPessoa.Tests.Controllers
{
    public class PessoaControllerTestes : IClassFixture<DbIntegrationFixture>
    {
        private readonly Mock<DataContext> _mockContext;
        private readonly PessoaController _controller;

        public PessoaControllerTestes(DbIntegrationFixture fixture)
        {
            _mockContext = new Mock<DataContext>();
            _controller = new PessoaController(_mockContext.Object);
        }

        //[Fact]
        //public async Task GetAll_RetornaListaDePessoas()
        //{
        //    // Arrange
        //    var pessoas = new List<Pessoa>
        //    {
        //        new Pessoa { Id = 1, Nome = "João" },
        //        new Pessoa { Id = 2, Nome = "Maria" }
        //    }.AsQueryable();

        //    _mockContext.Setup(x => x.Pessoas).Returns(DbContextMock.CreateDbSet(pessoas));

        //    // Act
        //    var resultado = await _controller.GetAll();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        //    var listaPessoas = Assert.IsType<List<Pessoa>>(okResult.Value);

        //    Assert.Equal(2, listaPessoas.Count);
        //    Assert.Equal("João", listaPessoas.First().Nome);
        //}

        //[Fact]
        //public async Task GetById_RetornaPessoaPorId()
        //{
        //    // Arrange
        //    var pessoa = new Pessoa { Id = 1, Nome = "João" };
        //    var pessoas = new List<Pessoa> { pessoa }.AsQueryable();

        //    _mockContext.Setup(x => x.Pessoas).Returns(DbContextMock.CreateDbSet(pessoas));

        //    // Act
        //    var resultado = await _controller.GetById(1);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
        //    var pessoaRetornada = Assert.IsType<Pessoa>(okResult.Value);

        //    Assert.Equal(1, pessoaRetornada.Id);
        //    Assert.Equal("João", pessoaRetornada.Nome);
        //}
    }
}