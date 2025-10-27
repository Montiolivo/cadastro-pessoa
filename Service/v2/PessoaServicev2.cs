using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service.Interfaces.v2;
using Cadastro_Pessoa.Services;
using Microsoft.EntityFrameworkCore;

namespace Cadastro_Pessoa.Service.v2;
    public class PessoaServiceV2 : IPessoaServiceV2
    {
        private readonly DataContext _context;

        public PessoaServiceV2(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PessoaV2Dto>> GetAllAsync()
        {
            return await _context.Pessoas
                .Include(p => p.Endereco)
                .Select(p => new PessoaV2Dto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Sexo = p.Sexo,
                    Email = p.Email,
                    DataNascimento = p.DataNascimento,
                    Naturalidade = p.Naturalidade,
                    Nacionalidade = p.Nacionalidade,
                    CPF = p.CPF,
                    Endereco = p.Endereco
                })
                .ToListAsync();
        }

        public async Task<PessoaV2Dto> GetByIdAsync(int id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
                throw new KeyNotFoundException("Pessoa não encontrada.");

            return new PessoaV2Dto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                DataNascimento = pessoa.DataNascimento,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                CPF = pessoa.CPF,
                Endereco = pessoa.Endereco
            };
        }

        public async Task<PessoaV2Dto> CreateAsync(PessoaV2Dto dto)
        {
            if (dto.Endereco == null)
                throw new ArgumentException("Endereço é obrigatório.");

            PessoaValidator.ValidarPessoa(new Pessoa { Nome = dto.Nome, CPF = dto.CPF }, _context);

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Sexo = dto.Sexo,
                Email = dto.Email,
                DataNascimento = dto.DataNascimento,
                Naturalidade = dto.Naturalidade,
                Nacionalidade = dto.Nacionalidade,
                CPF = dto.CPF,
                Endereco = dto.Endereco,
                DataCadastro = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            dto.Id = pessoa.Id;
            return dto;
        }

        public async Task<PessoaV2Dto> UpdateAsync(int id, PessoaV2Dto dto)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
                throw new KeyNotFoundException("Pessoa não encontrada.");

            PessoaValidator.ValidarPessoa(new Pessoa { Nome = dto.Nome, CPF = dto.CPF }, _context, id);

            pessoa.Nome = dto.Nome;
            pessoa.Sexo = dto.Sexo;
            pessoa.Email = dto.Email;
            pessoa.DataNascimento = dto.DataNascimento;
            pessoa.Naturalidade = dto.Naturalidade;
            pessoa.Nacionalidade = dto.Nacionalidade;
            pessoa.CPF = dto.CPF;
            pessoa.DataAtualizacao = DateTime.UtcNow;

            if (dto.Endereco == null)
                throw new ArgumentException("Endereço é obrigatório.");

            pessoa.Endereco = dto.Endereco;

            await _context.SaveChangesAsync();

            dto.Id = pessoa.Id;
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                throw new KeyNotFoundException("Pessoa não encontrada.");

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
        }
    }

