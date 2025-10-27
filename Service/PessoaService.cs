using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;
using Cadastro_Pessoa.Service.Interfaces;
using Cadastro_Pessoa.Services;
using Microsoft.EntityFrameworkCore;

namespace Cadastro_Pessoa.Service
{
    public class PessoaService : IPessoaService
    {
        private readonly DataContext _context;

        public PessoaService(DataContext context)
        {
            _context = context;
        }

        // Retorna todos como DTO v1 (sem endereço)
        public async Task<IEnumerable<PessoaV1Dto>> GetAllAsync()
        {
            return await _context.Pessoas
                .Select(p => new PessoaV1Dto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Sexo = p.Sexo,
                    Email = p.Email,
                    DataNascimento = p.DataNascimento,
                    Naturalidade = p.Naturalidade,
                    Nacionalidade = p.Nacionalidade,
                    CPF = p.CPF
                }).ToListAsync();
        }

        // Retorna por id como DTO v1
        public async Task<PessoaV1Dto> GetByIdAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) throw new KeyNotFoundException("Pessoa não encontrada.");

            return new PessoaV1Dto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                DataNascimento = pessoa.DataNascimento,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                CPF = pessoa.CPF
            };
        }

        // Cria usando o modelo Pessoa
        public async Task<Pessoa> CreateAsync(PessoaV1Dto dto)
        {
            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Sexo = dto.Sexo,
                Email = dto.Email,
                DataNascimento = dto.DataNascimento,
                Naturalidade = dto.Naturalidade,
                Nacionalidade = dto.Nacionalidade,
                CPF = dto.CPF,
                DataCadastro = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
                // Endereço não é definido na v1
            };

            PessoaValidator.ValidarPessoa(pessoa, _context);

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(int id, PessoaV1Dto dto)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) throw new KeyNotFoundException("Pessoa não encontrada.");

            PessoaValidator.ValidarPessoa(new Pessoa
            {
                Nome = dto.Nome,
                CPF = dto.CPF
            }, _context, id);

            pessoa.Nome = dto.Nome;
            pessoa.Sexo = dto.Sexo;
            pessoa.Email = dto.Email;
            pessoa.DataNascimento = dto.DataNascimento;
            pessoa.Naturalidade = dto.Naturalidade;
            pessoa.Nacionalidade = dto.Nacionalidade;
            pessoa.CPF = dto.CPF;
            pessoa.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task DeleteAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) throw new KeyNotFoundException("Pessoa não encontrada.");

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
        }
    }
}
