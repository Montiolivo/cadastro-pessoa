using Cadastro_Pessoa.Models;

namespace Cadastro_Pessoa.Services;
public interface IPessoaService
{
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa> GetByIdAsync(int id);
    Task<Pessoa> CreateAsync(Pessoa pessoa);
    Task<Pessoa> UpdateAsync(int id, Pessoa pessoa);
    Task DeleteAsync(int id);
}

