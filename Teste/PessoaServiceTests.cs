using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Services;
using CadastroPessoa.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CadastroPessoa.Tests.Services
{
    public class PessoaServiceTests
    {
        private readonly DataContext _context;
        private readonly PessoaService _service;

        public PessoaServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _context.Pessoas.AddRange(PessoaMock.GetPessoas());
            _context.SaveChanges();

            _service = new PessoaService(_context);
        }

        [Fact]
        public async Task GetAllAsync_RetornaTodasAsPessoas()
        {
            // Act
            var pessoas = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(pessoas);
            Assert.Equal(3, pessoas.Count());
            Assert.Equal("João Silva", pessoas.First().Nome);
        }

        [Fact]
        public async Task GetByIdAsync_RetornaPessoaPorId()
        {
            // Act
            var pessoa = await _service.GetByIdAsync(2);

            // Assert
            Assert.NotNull(pessoa);
            Assert.Equal(2, pessoa.Id);
            Assert.Equal("Maria Oliveira", pessoa.Nome);
        }

        [Fact]
        public async Task CreateAsync_AdicionaNovaPessoa()
        {
            // Arrange
            var novaPessoa = new Pessoa
            {
                Nome = "Ana Souza",
                CPF = "98765432100",
                DataNascimento = new DateTime(1995, 10, 10),
                Email = "ana.souza@email.com",
                Sexo = "Feminino",
                Naturalidade = "Salvador",
                Nacionalidade = "Brasileira"
            };

            // Act
            var pessoaCriada = await _service.CreateAsync(novaPessoa);

            // Assert
            Assert.NotNull(pessoaCriada);
            Assert.Equal("Ana Souza", pessoaCriada.Nome);
            Assert.Equal(4, await _context.Pessoas.CountAsync()); 
        }

        [Fact]
        public async Task UpdateAsync_AtualizaPessoaExistente()
        {
            // Arrange
            var pessoaAtualizada = new Pessoa
            {
                Nome = "Maria Oliveira Atualizada",
                CPF = "40270925031",
                DataNascimento = new DateTime(1985, 8, 15),
                Email = "maria.nova@email.com",
                Sexo = "Feminino",
                Naturalidade = "Rio de Janeiro",
                Nacionalidade = "Brasileira"
            };

            // Act
            var pessoa = await _service.UpdateAsync(2, pessoaAtualizada);

            // Assert
            Assert.NotNull(pessoa);
            Assert.Equal("Maria Oliveira Atualizada", pessoa.Nome);
            Assert.Equal("maria.nova@email.com", pessoa.Email);
        }

        [Fact]
        public async Task DeleteAsync_RemovePessoaExistente()
        {
            // Act
            await _service.DeleteAsync(1);

            // Assert
            Assert.Equal(2, await _context.Pessoas.CountAsync()); 
            var pessoa = await _context.Pessoas.FindAsync(1);
            Assert.Null(pessoa);
        }
    }
}
