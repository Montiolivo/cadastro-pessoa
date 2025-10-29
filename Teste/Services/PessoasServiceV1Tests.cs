using AutoMapper;
using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CadastroPessoa.Tests.Services
{
    public class PessoaServiceV1Tests
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly PessoaService _service;
        private readonly ILoggerFactory _loggerFactory;

        public PessoaServiceV1Tests()
        {

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new DataContext(options);

            _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pessoa, PessoaV1Dto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            }, _loggerFactory);

            _mapper = config.CreateMapper();

            _service = new PessoaService(_context, _mapper);
        }


        [Fact]
        public async Task CreateAsync_DeveCriarPessoa_ComSucesso()
        {
            var dto = new PessoaV1Dto
            {
                Nome = "Miguel",
                CPF = "65585493019",
                DataNascimento = new DateTime(2000, 1, 1)
            };

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Miguel", result.Nome);
            Assert.NotEqual(default, result.Id);
            Assert.True(await _context.Pessoas.AnyAsync(p => p.Nome == "Miguel"));
        }


        [Fact]
        public async Task GetAllAsync_DeveRetornarTodasPessoas()
        {
            _context.Pessoas.AddRange(
                new Pessoa { Nome = "Pessoa 1", CPF = "87379985019", DataNascimento = DateTime.Now },
                new Pessoa { Nome = "Pessoa 2", CPF = "66003008091", DataNascimento = DateTime.Now }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();

            Assert.NotEmpty(result);
            Assert.Equal(2, ((List<PessoaV1Dto>)result).Count);
        }


        [Fact]
        public async Task GetByIdAsync_DeveRetornarPessoa_QuandoExiste()
        {
            var pessoa = new Pessoa { Nome = "João", CPF = "81644819040", DataNascimento = DateTime.Now };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(pessoa.Id);

            Assert.NotNull(result);
            Assert.Equal("João", result.Nome);
        }

        [Fact]
        public async Task GetByIdAsync_DeveLancarExcecao_QuandoNaoEncontrar()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(999));
        }


        [Fact]
        public async Task UpdateAsync_DeveAtualizarPessoa_QuandoExistente()
        {
            var pessoa = new Pessoa { Nome = "Antigo", CPF = "49538148002", DataNascimento = DateTime.Now };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var dto = new PessoaV1Dto { Nome = "Novo Nome", CPF = "40710053002", DataNascimento = pessoa.DataNascimento };

            var result = await _service.UpdateAsync(pessoa.Id, dto);

            Assert.Equal("Novo Nome", result.Nome);
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SePessoaNaoExistir()
        {
            var dto = new PessoaV1Dto { Nome = "Novo", CPF = "59727562000", DataNascimento = DateTime.Now };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, dto));
        }

        [Fact]
        public async Task DeleteAsync_DeveRemoverPessoa_QuandoExistir()
        {
            var pessoa = new Pessoa { Nome = "Apagar", CPF = "16467931025", DataNascimento = DateTime.Now };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            await _service.DeleteAsync(pessoa.Id);

            var pessoaNoBanco = await _context.Pessoas.FindAsync(pessoa.Id);
            Assert.Null(pessoaNoBanco);
        }

        [Fact]
        public async Task DeleteAsync_DeveLancarExcecao_SeNaoEncontrar()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(999));
        }
    }
}
