using Cadastro_Pessoa.Data;
using Cadastro_Pessoa.Models;
using Microsoft.EntityFrameworkCore;

namespace Cadastro_Pessoa.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly DataContext _context;

        public PessoaService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pessoa>> GetAllAsync()
        {
            return await _context.Pessoas.ToListAsync();
        }

        public async Task<Pessoa> GetByIdAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) throw new KeyNotFoundException("Pessoa não encontrada.");
            return pessoa;
        }

        public async Task<Pessoa> CreateAsync(Pessoa pessoa)
        {
            PessoaValidator.ValidarPessoa(pessoa, _context);

            pessoa.DataCadastro = DateTime.UtcNow;
            pessoa.DataAtualizacao = DateTime.UtcNow;

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(int id, Pessoa pessoaAtualizada)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) throw new KeyNotFoundException("Pessoa não encontrada.");

            PessoaValidator.ValidarPessoa(pessoaAtualizada, _context, id);

            pessoa.Nome = pessoaAtualizada.Nome;
            pessoa.Sexo = pessoaAtualizada.Sexo;
            pessoa.Email = pessoaAtualizada.Email;
            pessoa.DataNascimento = pessoaAtualizada.DataNascimento;
            pessoa.Naturalidade = pessoaAtualizada.Naturalidade;
            pessoa.Nacionalidade = pessoaAtualizada.Nacionalidade;
            pessoa.CPF = pessoaAtualizada.CPF;
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
