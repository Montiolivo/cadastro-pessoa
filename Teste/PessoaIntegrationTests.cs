using Cadastro_Pessoa.Controllers;
using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CadastroPessoa.Tests.IntegrationTests
{
    public class PessoasIntegrationTests : IClassFixture<DbIntegrationFixture>
    {
        private readonly DataContext _context;
        private readonly PessoaController _controller;

        public PessoasIntegrationTests(DbIntegrationFixture fixture)
        {
            _context = fixture.Context;
            _controller = new PessoaController(_context);
        }

        [Fact]
        public async Task Create_RetornaPessoaCriada()
        {
            // Arrange
            var pessoa = new Pessoa
            {
                Nome = "Ana",
                DataNascimento = DateTime.Now.AddYears(-20),
                CPF = "12345678901"
            };

            // Act
            var resultado = await _controller.Create(pessoa);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(resultado);
            var pessoaCriada = Assert.IsType<Pessoa>(createdResult.Value);

            Assert.NotNull(pessoaCriada);
            Assert.Equal("Ana", pessoaCriada.Nome);
            Assert.Equal(1, await _context.Pessoas.CountAsync());
        }
    }
}