using Cadastro_Pessoa.Models;
using Cadastro_Pessoa.Models.DTO;

namespace Cadastro_Pessoa.Service.Interfaces;
public interface IPessoaService
{
    Task<IEnumerable<PessoaV1Dto>> GetAllAsync();
    Task<PessoaV1Dto> GetByIdAsync(int id);
    Task<Pessoa> CreateAsync(PessoaV1Dto dto);
    Task<Pessoa> UpdateAsync(int id, PessoaV1Dto dto);
    Task DeleteAsync(int id);
}

