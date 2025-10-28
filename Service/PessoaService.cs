using AutoMapper;
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
        private readonly IMapper _mapper;

        public PessoaService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PessoaV1Dto>> GetAllAsync() =>
            _mapper.Map<IEnumerable<PessoaV1Dto>>(await _context.Pessoas.ToListAsync());


        public async Task<PessoaV1Dto> GetByIdAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            return pessoa == null ? 
                throw new KeyNotFoundException("Pessoa não encontrada.") : 
                _mapper.Map<PessoaV1Dto>(pessoa);
        }

        public async Task<PessoaV1Dto> CreateAsync(PessoaV1Dto dto)
        {
            var pessoa = _mapper.Map<Pessoa>(dto);

            pessoa.DataCadastro = DateTime.UtcNow;
            pessoa.DataAtualizacao = DateTime.UtcNow;

            PessoaValidator.ValidarPessoa(pessoa, _context);

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return _mapper.Map<PessoaV1Dto>(pessoa);
        }

        public async Task<PessoaV1Dto> UpdateAsync(int id, PessoaV1Dto dto)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                throw new KeyNotFoundException("Pessoa não encontrada.");

            PessoaValidator.ValidarPessoa(
                new Pessoa
                {
                    Nome = dto.Nome,
                    CPF = dto.CPF,
                    DataNascimento = dto.DataNascimento
                }, _context, id);

            _mapper.Map(dto, pessoa);
            pessoa.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<PessoaV1Dto>(pessoa);
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
