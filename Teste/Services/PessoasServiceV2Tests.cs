using AutoMapper;
using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service.v2;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CadastroPessoa.Tests.Services
{
    public class PessoaServiceV2Tests
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly PessoaServiceV2 _service;
        private readonly ILoggerFactory _loggerFactory;

        public PessoaServiceV2Tests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pessoa, PessoaV2Dto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            }, _loggerFactory);

            _mapper = config.CreateMapper();
            _service = new PessoaServiceV2(_context, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDePessoas()
        {
            _context.Pessoas.AddRange(
                new Pessoa
                {
                    Nome = "Pessoa 1",
                    Sexo = "M",
                    Email = "pessoa1@email.com",
                    DataNascimento = new DateTime(1990, 1, 1),
                    Naturalidade = "São Paulo",
                    Nacionalidade = "Brasileira",
                    CPF = "88287864003",
                    Endereco = new Endereco
                    {
                        Logradouro = "Rua A",
                        Numero = "100",
                        Complemento = "Casa",
                        Bairro = "Centro",
                        Cidade = "São Paulo",
                        Estado = "SP",
                        CEP = "01001000"
                    }
                },
                new Pessoa
                {
                    Nome = "Pessoa 2",
                    Sexo = "F",
                    Email = "pessoa2@email.com",
                    DataNascimento = new DateTime(1995, 5, 5),
                    Naturalidade = "Curitiba",
                    Nacionalidade = "Brasileira",
                    CPF = "72797427009",
                    Endereco = new Endereco
                    {
                        Logradouro = "Rua B",
                        Numero = "200",
                        Complemento = "Apto 2",
                        Bairro = "Batel",
                        Cidade = "Curitiba",
                        Estado = "PR",
                        CEP = "80020000"
                    }
                }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();

            Assert.NotEmpty(result);
            Assert.Collection(result,
                p => Assert.Equal("Pessoa 1", p.Nome),
                p => Assert.Equal("Pessoa 2", p.Nome));
        }


        [Fact]
        public async Task GetByIdAsync_DeveRetornarPessoa_QuandoExistente()
        {
            var pessoa = new Pessoa
            {
                Nome = "Maria",
                Sexo = "F",
                Email = "maria@email.com",
                DataNascimento = new DateTime(1988, 3, 3),
                Naturalidade = "Florianópolis",
                Nacionalidade = "Brasileira",
                CPF = "81637533071",
                Endereco = new Endereco
                {
                    Logradouro = "Rua das Flores",
                    Numero = "123",
                    Complemento = "Casa",
                    Bairro = "Centro",
                    Cidade = "Florianópolis",
                    Estado = "SC",
                    CEP = "88000000"
                }
            };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(pessoa.Id);

            Assert.NotNull(result);
            Assert.Equal("Maria", result.Nome);
            Assert.Equal("Rua das Flores", result.Endereco!.Logradouro);
            Assert.Equal("SC", result.Endereco.Estado);
        }

        [Fact]
        public async Task GetByIdAsync_DeveLancarExcecao_SeNaoEncontrar()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(999));
        }


        [Fact]
        public async Task CreateAsync_DeveCriarPessoa_ComEnderecoCompleto()
        {
            var dto = new PessoaV2Dto
            {
                Nome = "João Silva",
                Sexo = "M",
                Email = "joao@email.com",
                DataNascimento = new DateTime(1999, 12, 12),
                Naturalidade = "Rio de Janeiro",
                Nacionalidade = "Brasileira",
                CPF = "26795975088",
                Endereco = new Endereco
                {
                    Logradouro = "Rua Azul",
                    Numero = "55",
                    Complemento = "Bloco B",
                    Bairro = "Copacabana",
                    Cidade = "Rio de Janeiro",
                    Estado = "RJ",
                    CEP = "22041001"
                }
            };

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("João Silva", result.Nome);
            Assert.Equal("Rua Azul", result.Endereco!.Logradouro);
            Assert.True(await _context.Pessoas.AnyAsync(p => p.CPF == "26795975088"));
        }

        [Fact]
        public async Task CreateAsync_DeveLancarExcecao_SeEnderecoForNulo()
        {
            var dto = new PessoaV2Dto
            {
                Nome = "Sem Endereço",
                Sexo = "M",
                Email = "sem@endereco.com",
                DataNascimento = DateTime.UtcNow.AddYears(-20),
                Naturalidade = "Fortaleza",
                Nacionalidade = "Brasileira",
                CPF = "77244025076",
                Endereco = null
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        }


        [Fact]
        public async Task UpdateAsync_DeveAtualizarPessoa_ComSucesso()
        {
            var pessoa = new Pessoa
            {
                Nome = "Antigo",
                Sexo = "M",
                Email = "antigo@email.com",
                DataNascimento = new DateTime(1990, 1, 1),
                Naturalidade = "Natal",
                Nacionalidade = "Brasileira",
                CPF = "37032091008",
                Endereco = new Endereco
                {
                    Logradouro = "Rua Velha",
                    Numero = "10",
                    Bairro = "Centro",
                    Cidade = "Natal",
                    Estado = "RN",
                    CEP = "59000000"
                }
            };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var dto = new PessoaV2Dto
            {
                Nome = "Novo Nome",
                Sexo = "M",
                Email = "novo@email.com",
                DataNascimento = pessoa.DataNascimento,
                Naturalidade = "Natal",
                Nacionalidade = "Brasileira",
                CPF = "09019529072",
                Endereco = new Endereco
                {
                    Logradouro = "Rua Nova",
                    Numero = "99",
                    Complemento = "Casa",
                    Bairro = "Tirol",
                    Cidade = "Natal",
                    Estado = "RN",
                    CEP = "59020000"
                }
            };

            var result = await _service.UpdateAsync(pessoa.Id, dto);

            Assert.Equal("Novo Nome", result.Nome);
            Assert.Equal("Rua Nova", result.Endereco!.Logradouro);
            Assert.Equal("99", result.Endereco.Numero);
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SePessoaNaoExistir()
        {
            var dto = new PessoaV2Dto
            {
                Nome = "Inexistente",
                Sexo = "M",
                Email = "inexistente@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                Naturalidade = "Belém",
                Nacionalidade = "Brasileira",
                CPF = "59579451028",
                Endereco = new Endereco
                {
                    Logradouro = "Rua Teste",
                    Numero = "1",
                    Bairro = "Centro",
                    Cidade = "Belém",
                    Estado = "PA",
                    CEP = "66000000"
                }
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, dto));
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_SeEnderecoForNulo()
        {
            var pessoa = new Pessoa
            {
                Nome = "Sem Endereço",
                Sexo = "F",
                Email = "teste@endereco.com",
                DataNascimento = new DateTime(2000, 5, 5),
                Naturalidade = "Salvador",
                Nacionalidade = "Brasileira",
                CPF = "97500733046",
                Endereco = new Endereco
                {
                    Logradouro = "Rua Antiga",
                    Numero = "9",
                    Bairro = "Pituba",
                    Cidade = "Salvador",
                    Estado = "BA",
                    CEP = "40000000"
                }
            };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var dto = new PessoaV2Dto
            {
                Nome = "Sem Endereço Atualizado",
                Sexo = "F",
                Email = "teste@endereco.com",
                DataNascimento = pessoa.DataNascimento,
                Naturalidade = "Salvador",
                Nacionalidade = "Brasileira",
                CPF = "55229592079",
                Endereco = null
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(pessoa.Id, dto));
        }

       
        [Fact]
        public async Task DeleteAsync_DeveRemoverPessoa()
        {
            var pessoa = new Pessoa
            {
                Nome = "Excluir",
                Sexo = "M",
                Email = "excluir@email.com",
                DataNascimento = new DateTime(1992, 10, 10),
                Naturalidade = "João Pessoa",
                Nacionalidade = "Brasileira",
                CPF = "45982327069",
                Endereco = new Endereco
                {
                    Logradouro = "Rua do Sol",
                    Numero = "777",
                    Bairro = "Centro",
                    Cidade = "João Pessoa",
                    Estado = "PB",
                    CEP = "58000000"
                }
            };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            await _service.DeleteAsync(pessoa.Id);

            var existe = await _context.Pessoas.AnyAsync(p => p.Id == pessoa.Id);
            Assert.False(existe);
        }

        [Fact]
        public async Task DeleteAsync_DeveLancarExcecao_SeNaoEncontrar()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(1234));
        }
    }
}
